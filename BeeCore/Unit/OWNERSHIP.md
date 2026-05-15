# BeeCore Unit Ownership

`BeeCore/Unit` owns vision tool models and processing behavior used by runtime
inspection and calibration.

Units may expose parameters and results consumed by WinForms tool forms, but they
should not own operator UI controls, modal dialogs, or WinForms thread-marshalling
policy. UI orchestration belongs in `BeeInterface/Tool`.

