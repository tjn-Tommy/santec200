"""Benchmark: single optical NL layer on raw 28x28 MNIST pixels.

Direct apples-to-apples counterpart of benchmark_linear.py.  IDENTICAL structure
(784 -> 10, flat input, same Adam + cosine LR, same CrossEntropyLoss, same JSON
schema) -- the ONLY difference is the layer:

    linear  :  logit_k = sum_i  W[k,i] * x_i              (real weighted sum)
    optical :  logit_k = | sum_i  W~[k,i] * x~_i |^2       (coherent sum + |.|^2)

  where  x~_i = amplitudes_to_efield(x_i)   (pixel -> E-field, |x~|=x)
         W~[k,i] = exp-shaped E-field of the learnable phase phi_w[k,i]

Input encoding: flat 784 vector for BOTH models (no 4x4 patches).  A single
fully-connected layer gives every pixel its own weight, so pixel order is
irrelevant -- flatten vs patch makes no difference here.  No saturation.

10 classes x 784 phases = 7,840 trainable params (same count as the linear model).

Usage
-----
  python -m rbONN.benchmark_optical
  python -m rbONN.benchmark_optical --epochs 80 --name optical_7840
"""
from __future__ import annotations

import argparse
import json
import math
from pathlib import Path

import numpy as np
import torch
import torch.nn as nn
from torch.utils.data import DataLoader, TensorDataset
from torchvision import datasets, transforms

from .twin import amplitudes_to_efield, phases_to_efield

DATA_DIR = Path("data")
OUTPUT_DIR = Path("outputs/rbONN")
N_CLASSES = 10


def _load_mnist(root: Path = DATA_DIR):
    t = transforms.ToTensor()
    tr = datasets.MNIST(root, train=True, download=True, transform=t)
    te = datasets.MNIST(root, train=False, download=True, transform=t)
    X_tr = tr.data.numpy().reshape(-1, 784).astype(np.float32) / 255.0
    X_te = te.data.numpy().reshape(-1, 784).astype(np.float32) / 255.0
    return X_tr, tr.targets.numpy(), X_te, te.targets.numpy()


def _confusion_matrix(pred: torch.Tensor, truth: torch.Tensor) -> np.ndarray:
    cm = np.zeros((N_CLASSES, N_CLASSES), dtype=int)
    for t, p in zip(truth.cpu().numpy(), pred.cpu().numpy()):
        cm[t, p] += 1
    return cm


class OpticalLayer(nn.Module):
    """One optical neuron per class: global coherent sum over all 784 pixels.

    Identical fan-in to nn.Linear(784, 10, bias=False) -- 7,840 phase weights --
    but with complex E-field weights and square-law (|.|^2) detection instead of
    a real dot product.  No nonlinearity beyond the physical square-law.
    """

    def __init__(self, n_in: int = 784, n_out: int = N_CLASSES):
        super().__init__()
        self.phi_w = nn.Parameter(torch.rand(n_out, n_in) * 2.0 * math.pi)

    def forward(self, a: torch.Tensor) -> torch.Tensor:
        x = amplitudes_to_efield(a)        # (batch, 784) complex, |x| = pixel
        W = phases_to_efield(self.phi_w)   # (n_out, 784) complex
        S = x @ W.T                        # (batch, n_out) complex coherent sum
        return S.abs().pow(2)              # (batch, n_out) square-law logits


def train_optical(
    epochs: int = 80,
    batch_size: int = 256,
    lr: float = 1e-2,
    name: str = "optical_7840",
) -> dict:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    gpu = torch.cuda.get_device_name(0) if device.type == "cuda" else ""
    print(f"Device: {device}" + (f" ({gpu})" if gpu else ""))

    X_tr, y_tr, X_te, y_te = _load_mnist()
    A_tr = torch.from_numpy(X_tr).to(device)
    A_te = torch.from_numpy(X_te).to(device)
    y_tr_t = torch.tensor(y_tr, dtype=torch.long, device=device)
    y_te_t = torch.tensor(y_te, dtype=torch.long, device=device)

    model = OpticalLayer(784, N_CLASSES).to(device)
    n_params = sum(p.numel() for p in model.parameters())
    print(f"Model: optical NL 784 -> {N_CLASSES}  |S|^2  |  {n_params} parameters")

    loader = DataLoader(TensorDataset(A_tr, y_tr_t), batch_size=batch_size, shuffle=True)
    optimizer = torch.optim.Adam(model.parameters(), lr=lr)
    scheduler = torch.optim.lr_scheduler.CosineAnnealingLR(optimizer, T_max=epochs, eta_min=1e-4)
    criterion = nn.CrossEntropyLoss()

    best_acc, best_state, history = 0.0, None, []
    for epoch in range(epochs):
        model.train()
        running = 0.0
        for xb, yb in loader:
            optimizer.zero_grad()
            loss = criterion(model(xb), yb)
            loss.backward()
            torch.nn.utils.clip_grad_norm_(model.parameters(), max_norm=1.0)
            optimizer.step()
            running += loss.item() * len(xb)
        scheduler.step()

        model.eval()
        with torch.no_grad():
            acc = (model(A_te).argmax(1) == y_te_t).float().mean().item()
        avg_loss = running / len(A_tr)
        if acc > best_acc:
            best_acc, best_state = acc, {k: v.clone() for k, v in model.state_dict().items()}

        if (epoch + 1) % 10 == 0 or epoch == 0 or epoch == epochs - 1:
            history.append({
                "epoch": epoch + 1,
                "train_loss": round(avg_loss, 6),
                "test_acc": round(acc, 6),
                "best_acc": round(best_acc, 6),
                "lr": round(optimizer.param_groups[0]["lr"], 8),
            })
            print(f"  epoch {epoch+1:4d}/{epochs}  loss={avg_loss:.4f}  "
                  f"acc={acc:.2%}  best={best_acc:.2%}")

    if best_state is not None:
        model.load_state_dict(best_state)
    model.eval()
    with torch.no_grad():
        pred = model(A_te).argmax(1)
    cm = _confusion_matrix(pred, y_te_t)

    metrics_path = OUTPUT_DIR / f"metrics_{name}.json"
    with open(metrics_path, "w") as f:
        json.dump({
            "run_name": name,
            "detection_mode": "intensity",
            "encoding": "raw_784px_flat_optical_NL",
            "n_params": n_params,
            "best_acc": best_acc,
            "epochs": epochs,
            "per_class_acc": {str(k): float(cm[k, k] / cm[k].sum()) for k in range(N_CLASSES)},
            "history": history,
        }, f, indent=2)

    print(f"\nBest test accuracy: {best_acc:.2%}  ({n_params} params)")
    print(f"Metrics -> {metrics_path}")
    return {"best_acc": best_acc, "cm": cm}


def main():
    p = argparse.ArgumentParser(description=__doc__,
                                formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--epochs", type=int, default=80)
    p.add_argument("--lr", type=float, default=1e-2)
    p.add_argument("--batch-size", type=int, default=256)
    p.add_argument("--name", type=str, default="optical_7840")
    args = p.parse_args()
    train_optical(epochs=args.epochs, batch_size=args.batch_size, lr=args.lr, name=args.name)


if __name__ == "__main__":
    main()
