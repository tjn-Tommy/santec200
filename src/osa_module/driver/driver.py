from __future__ import annotations

import socket
import time

import numpy as np

from .base_osa import BaseOSA, OSAConnectionError, OSAError, OSATimeoutError

# Bit 0 of :STATus:OPERation:EVENt? is set when a sweep finishes (Guide 7.x).
_SWEEP_DONE_BIT = 0x01


class AQ637X_Driver(BaseOSA):
    """Yokogawa AQ637X-series OSA over a raw TCP socket (default port 10001).

    Mirrors the documented remote flow used by read_trace.ipynb:
    telnet-style authentication (open "<user>" -> password line -> "ready"),
    then SCPI-style commands in the AQ637X command format (CFORM1).

    The transport is newline-terminated ASCII: commands are sent with a
    trailing CR/LF and replies are read until LF. Every public method is
    serialized by the io lock inherited from BaseOSA, so a sweep poll on one
    thread never interleaves with a trace download on another.
    """

    name = "AQ637X"

    def __init__(
        self,
        host: str,
        port: int = 10001,
        *,
        timeout: float = 5.0,
        username: str = "anonymous",
        password: str = "",
        command_format: int = 1,
        write_delay: float = 0.1,
        buffer_size: int = 8192,
    ):
        super().__init__()
        self.host = str(host)
        self.port = int(port)
        self.timeout = float(timeout)
        self.username = str(username)
        self.password = str(password)
        self.command_format = int(command_format)
        self.write_delay = float(write_delay)
        self.buffer_size = int(buffer_size)
        self._sock: socket.socket | None = None

    # --- transport hooks --------------------------------------------------
    def _open_transport(self) -> None:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.settimeout(self.timeout)
        self._sock = sock
        try:
            sock.connect((self.host, self.port))
            self._authenticate()
            # AQ637X command format, so the documented SCPI commands apply.
            self._write(f"CFORM{self.command_format}")
        except OSAError:
            self._discard_socket()
            raise
        except OSError as exc:
            self._discard_socket()
            raise OSAConnectionError(
                f"failed to connect to OSA at {self.host}:{self.port}: {exc}"
            ) from exc

    def _authenticate(self) -> None:
        # Telnet-style handshake (read_trace.ipynb): send the user name, then
        # an (empty for anonymous) password line, and expect "ready". The
        # prompts are short and need not be newline-terminated, so the
        # handshake uses single recv() calls rather than the line reader.
        self._raw_send(f'open "{self.username}"')
        self._raw_recv_once()  # username prompt / echo, discarded
        self._raw_send(self.password)
        reply = self._raw_recv_once()
        if not reply.lower().startswith("ready"):
            raise OSAConnectionError(
                f"OSA authentication failed; expected 'ready', got {reply!r}"
            )

    def _close_transport(self) -> None:
        self._discard_socket()

    def _discard_socket(self) -> None:
        if self._sock is not None:
            try:
                self._sock.close()
            finally:
                self._sock = None

    def _write(self, command: str) -> None:
        self._raw_send(command)
        if self.write_delay > 0:
            time.sleep(self.write_delay)

    def _query(self, command: str) -> str:
        self._raw_send(command)
        return self._raw_recv()

    # --- low-level socket helpers (assume the io lock is held) -----------
    def _raw_send(self, command: str) -> None:
        if self._sock is None:
            raise OSAConnectionError("socket is not open")
        try:
            self._sock.sendall(f"{command}\r\n".encode("ascii"))
        except socket.timeout as exc:
            raise OSATimeoutError(f"timed out sending {command!r}") from exc
        except OSError as exc:
            raise OSAConnectionError(f"failed sending {command!r}: {exc}") from exc

    def _raw_recv_once(self) -> str:
        """Read whatever is currently available (used only for the handshake)."""
        chunk = self._recv_chunk()
        return chunk.decode("ascii", errors="replace").strip()

    def _raw_recv(self) -> str:
        """Read a full newline-terminated reply."""
        data = b""
        while not data.endswith(b"\n"):
            data += self._recv_chunk()
        return data.decode("ascii", errors="replace").strip()

    def _recv_chunk(self) -> bytes:
        if self._sock is None:
            raise OSAConnectionError("socket is not open")
        try:
            chunk = self._sock.recv(self.buffer_size)
        except socket.timeout as exc:
            raise OSATimeoutError("timed out waiting for OSA reply") from exc
        except OSError as exc:
            raise OSAConnectionError(f"socket error while reading: {exc}") from exc
        if not chunk:
            raise OSAConnectionError("connection closed by OSA")
        return chunk

    # --- identification ---------------------------------------------------
    def identify(self) -> str:
        return self.query("*IDN?")

    # --- measurement configuration ---------------------------------------
    def set_center_wavelength(self, value: str) -> None:
        self.write(f":SENSe:WAVelength:CENTer {value}")

    def set_span(self, value: str) -> None:
        self.write(f":SENSe:WAVelength:SPAN {value}")

    def set_sensitivity(self, value: str) -> None:
        self.write(f":SENSe:SENSe {value}")

    def set_sampling_points(self, value: str | int) -> None:
        text = str(value)
        if text.upper() == "AUTO":
            self.write(":SENSe:SWEEp:POINts:AUTO ON")
        else:
            self.write(":SENSe:SWEEp:POINts:AUTO OFF")
            self.write(f":SENSe:SWEEp:POINts {text}")

    def set_y_unit(self, value: str) -> None:
        self.write(f":DISPlay:TRACe:Y:SPACing {value}")

    def set_reference_level(self, value: str) -> None:
        self.write(f":DISPlay:TRACe:Y:RLEVel {value}")

    def select_trace(
        self, trace_id: str, mode: str = "WRITe", clear: bool = True
    ) -> None:
        self.write(f":TRACe:ACTive {trace_id}")
        self.write(f":TRACe:ATTRibute:{trace_id} {mode}")
        if clear:
            self.write(f":TRACe:CLEar {trace_id}")

    # --- sweep control ----------------------------------------------------
    def initiate_single_sweep(self) -> None:
        self.write(":INITiate:SMODe 1")  # single-sweep mode
        self.write("*CLS")               # clear status registers
        self.write(":INITiate")          # start the sweep

    def operation_event(self) -> int:
        """Read (and clear) the operation event register."""
        return int(self.query(":STATus:OPERation:EVENt?"))

    def is_sweep_complete(self) -> bool:
        return bool(self.operation_event() & _SWEEP_DONE_BIT)

    # --- trace download ---------------------------------------------------
    def read_trace_x(self, trace_id: str) -> np.ndarray:
        return self._parse_block(self.query(f":TRACe:X? {trace_id}"))

    def read_trace_y(self, trace_id: str) -> np.ndarray:
        return self._parse_block(self.query(f":TRACe:Y? {trace_id}"))

    @staticmethod
    def _parse_block(raw: str, drop_count_header: bool = True) -> np.ndarray:
        """Parse a comma-separated ASCII block into a float array.

        read_trace.ipynb shows the first field is the point count rather than
        a data value, so it is dropped by default; set drop_count_header=False
        if a firmware variant returns pure data.
        """
        if not raw:
            return np.empty(0, dtype=float)
        values = [float(v) for v in raw.split(",") if v != ""]
        if drop_count_header and values:
            values = values[1:]
        return np.asarray(values, dtype=float)
