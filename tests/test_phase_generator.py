from __future__ import annotations

import sys
import tempfile
import unittest
from pathlib import Path

import numpy as np

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "src"))

from slm_module.controller import validate_slm_csv
from slm_module.generator import (
    export_center_scan_sequence,
    generate_center_scan,
    iter_center_scan_positions,
    make_vertical_window,
    write_santec_csv,
)


class PhaseGeneratorTests(unittest.TestCase):
    def test_make_vertical_window_places_level_and_clips_edge(self) -> None:
        data = make_vertical_window(width=8, height=3, x_start=6, level=700, window_px=5)

        self.assertEqual(data.shape, (3, 8))
        self.assertEqual(data.dtype, np.uint16)
        self.assertTrue(np.all(data[:, :6] == 0))
        self.assertTrue(np.all(data[:, 6:8] == 700))

    def test_scan_positions_are_inclusive_start_positions(self) -> None:
        positions = list(iter_center_scan_positions(20, step_px=5, start_x=0, end_x=12))

        self.assertEqual(positions, [0, 5, 10])

    def test_generate_center_scan_returns_patterns(self) -> None:
        patterns = list(
            generate_center_scan(
                width=12,
                height=2,
                level=512,
                window_px=5,
                step_px=5,
                start_x=0,
                end_x=10,
            )
        )

        self.assertEqual([pattern.x_start for pattern in patterns], [0, 5, 10])
        self.assertEqual(patterns[-1].x_end, 12)

    def test_write_santec_csv_matches_validator(self) -> None:
        data = make_vertical_window(width=6, height=4, x_start=2, level=1023, window_px=2)
        with tempfile.TemporaryDirectory() as temp_dir:
            path = write_santec_csv(data, Path(temp_dir) / "mask.csv")
            size = validate_slm_csv(path, expected_width=6, expected_height=4)

        self.assertEqual(size, (6, 4))

    def test_export_center_scan_sequence(self) -> None:
        with tempfile.TemporaryDirectory() as temp_dir:
            paths = export_center_scan_sequence(
                temp_dir,
                width=10,
                height=3,
                level=100,
                window_px=5,
                step_px=5,
            )

            self.assertEqual(len(paths), 2)
            for path in paths:
                self.assertTrue(path.exists())
                validate_slm_csv(path, expected_width=10, expected_height=3)


if __name__ == "__main__":
    unittest.main()
