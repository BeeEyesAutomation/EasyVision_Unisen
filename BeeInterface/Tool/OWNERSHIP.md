# BeeInterface Tool Ownership

`BeeInterface/Tool` owns WinForms controls for editing tool parameters, previewing
ROIs, and dispatching test or calibration commands from the operator UI.

Tool forms should translate UI state into `BeeCore.Unit` parameters and marshal UI
updates back to the WinForms thread. Vision algorithms, scoring rules, native
buffer ownership, and camera SDK calls belong in `BeeCore` or wrapper projects.

