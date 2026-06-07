from __future__ import annotations

import sys
import tempfile
import unittest
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "src"))

from slm_module.controller import SLMController, validate_slm_csv


class FakeDriver:
    def __init__(self, size: tuple[int, int] = (10, 4)):
        self.size = size
        self.loaded_csv: list[tuple[str, float]] = []
        self.grayscale: list[tuple[int, float]] = []
        self.opened = False

    def slm_info(self) -> tuple[int, int]:
        return self.size

    def open_slm(self) -> None:
        self.opened = True

    def close_slm(self) -> None:
        self.opened = False

    def load_grayscale(self, grayscale: int, interval: float = 0.2) -> None:
        self.grayscale.append((grayscale, interval))

    def load_csv(self, csv_path: str, interval: float = 0.2) -> None:
        self.loaded_csv.append((csv_path, interval))


class ControllerTests(unittest.TestCase):
    def test_display_grayscale_validates_level(self) -> None:
        fake = FakeDriver()
        controller = SLMController(driver=fake)

        controller.display_grayscale(123, interval=0.01)

        self.assertEqual(fake.grayscale, [(123, 0.01)])
        with self.assertRaises(ValueError):
            controller.display_grayscale(2000)

    def test_display_vertical_window_writes_and_loads_csv(self) -> None:
        fake = FakeDriver(size=(8, 3))
        controller = SLMController(driver=fake)
        with tempfile.TemporaryDirectory() as temp_dir:
            path = Path(temp_dir) / "window.csv"
            result = controller.display_vertical_window(
                x_start=2,
                level=400,
                window_px=5,
                csv_path=path,
                interval=0.01,
            )

            self.assertEqual(result, path.resolve())
            self.assertEqual(len(fake.loaded_csv), 1)
            validate_slm_csv(result, expected_width=8, expected_height=3)

    def test_display_center_scan_stops_when_event_is_set(self) -> None:
        fake = FakeDriver(size=(10, 2))
        controller = SLMController(driver=fake)
        seen: list[Path] = []

        def progress(index: int, path: Path) -> None:
            seen.append(path)
            if index == 0:
                stop_event.set()

        import threading

        stop_event = threading.Event()
        with tempfile.TemporaryDirectory() as temp_dir:
            paths = controller.display_center_scan(
                level=200,
                window_px=5,
                step_px=5,
                output_dir=temp_dir,
                stop_event=stop_event,
                progress_callback=progress,
                dwell_seconds=0.01,
            )

        self.assertEqual(len(paths), 1)
        self.assertEqual(len(seen), 1)
        self.assertEqual(len(fake.loaded_csv), 1)


if __name__ == "__main__":
    unittest.main()
