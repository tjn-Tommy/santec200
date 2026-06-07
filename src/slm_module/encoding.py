from __future__ import annotations

from typing import Protocol

import numpy as np


class EncodingStrategy(Protocol):
    name: str

    def encode(self, values: np.ndarray, shape: tuple[int, int]) -> np.ndarray:
        ...


class TPAEncodingStub:
    name = "TPA Multiplication"

    def encode(self, values: np.ndarray, shape: tuple[int, int]) -> np.ndarray:
        raise NotImplementedError("TPA multiplication encoding is not implemented yet")
