from __future__ import annotations

import threading
from abc import ABC, abstractmethod


class OSAError(RuntimeError):
    """Base error for OSA driver failures."""


class OSAConnectionError(OSAError):
    """Raised when the transport is not connected or drops unexpectedly."""


class OSATimeoutError(OSAError):
    """Raised when the OSA does not respond within the timeout."""


class BaseOSA(ABC):
    """Abstract base for optical spectrum analyzers.

    Concrete drivers implement the four transport hooks (_open_transport,
    _close_transport, _write, _query) and the vendor command set. The
    connection lifecycle and the thread-safe public write()/query() wrappers
    live here so every OSA -- regardless of transport (TCP, GPIB, USB) --
    shares one contract.

    A single re-entrant lock serializes every transaction. Unlike the SLM
    driver, whose display window is pinned to its creating thread and so needs
    a dedicated device thread, a request/response socket has no thread
    affinity; it only requires that a command and its reply are never
    interleaved with another thread's. A lock is the right tool for that and
    keeps every public method safe to call from any thread.
    """

    name: str = "osa"

    def __init__(self) -> None:
        self._io_lock = threading.RLock()
        self._connected = False

    @property
    def is_connected(self) -> bool:
        return self._connected

    # --- transport hooks implemented by concrete drivers -----------------
    @abstractmethod
    def _open_transport(self) -> None:
        """Open the link and perform any handshake; raise on failure."""

    @abstractmethod
    def _close_transport(self) -> None:
        """Release the underlying transport."""

    @abstractmethod
    def _write(self, command: str) -> None:
        """Send one command, no reply expected. Assumes the io lock is held."""

    @abstractmethod
    def _query(self, command: str) -> str:
        """Send one command and return its reply. Assumes the io lock is held."""

    # --- public, thread-safe API -----------------------------------------
    def connect(self) -> None:
        with self._io_lock:
            if self._connected:
                return
            self._open_transport()
            self._connected = True

    def disconnect(self) -> None:
        with self._io_lock:
            try:
                if self._connected:
                    self._close_transport()
            finally:
                self._connected = False

    def _ensure_connected(self) -> None:
        if not self._connected:
            raise OSAConnectionError(
                f"{self.name} is not connected; call connect() first"
            )

    def write(self, command: str) -> None:
        """Send a command that produces no reply."""
        with self._io_lock:
            self._ensure_connected()
            self._write(command)

    def query(self, command: str) -> str:
        """Send a command and return its single-line reply."""
        with self._io_lock:
            self._ensure_connected()
            return self._query(command)

    def __enter__(self):
        self.connect()
        return self

    def __exit__(self, exc_type, exc, tb):
        self.disconnect()
