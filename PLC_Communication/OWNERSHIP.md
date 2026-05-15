# PLC and industrial IO ownership

The PLC and industrial IO code is split across three areas. Use this file as the
local ownership guide until the folders are reorganized.

| Area | Owns | Should not own |
| --- | --- | --- |
| `PLC_Communication/` | Low-level HSL communication library code and shared PLC data pool helpers | WinForms configuration UI or EasyVision-specific protocol settings |
| `BeeGlobal/Protocol/` | EasyVision protocol models, PLC client facade, address/value configuration, and runtime protocol state | WinForms control layout or vendor-library implementation details |
| `BeeInterface/PLC/` | WinForms configuration screens and PLC input/output controls | Protocol storage, transport implementation, or reusable communication primitives |

Boundary rules:

- Keep UI changes in `BeeInterface/PLC`.
- Keep EasyVision protocol data and runtime client state in `BeeGlobal/Protocol`.
- Keep vendor/transport communication code in `PLC_Communication`.
- New PLC features should cross layers through existing public models or client
  methods instead of reaching into another layer's internal controls or helpers.
