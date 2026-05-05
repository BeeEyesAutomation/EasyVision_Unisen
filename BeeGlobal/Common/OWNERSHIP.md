# Common and utility ownership

Use this guide when adding or moving helper code. These areas overlap today, so
new helpers should follow the narrowest owner below.

| Area | Owns | Avoid adding |
| --- | --- | --- |
| `BeeGlobal/Common/` | Shared low-level UI/control helpers that are independent of a specific form or vision tool | Camera SDK calls, file/project loading, or tool-specific processing |
| `BeeGlobal/General/` | Dependency scanning and application-wide environment discovery | WinForms controls or BeeInterface-only helpers |
| `BeeInterface/Class/` | BeeInterface-specific UI, licensing, storage, keyboard, and integration helpers | BeeCore vision logic or reusable UI controls for BeeUi |
| `BeeInterface/Comunications/` | BeeInterface PLC/communication forms and their designer resources | Protocol models or low-level transport clients |
| `BeeUi/Commons/` | Reusable BeeUi controls and visual helpers | BeeInterface form logic or BeeCore processing state |
| `BeeCore/Func/` | Vision/core helpers, camera facades, native wrappers, drawing, cropping, result filtering, and image utilities | WinForms form orchestration or operator-screen state |
| `PLC_Communication/HslCommunication.*` | Vendor/transport communication library code | EasyVision UI state or protocol configuration screens |

Boundary rules:

- Prefer existing owner folders before creating a new utility location.
- Keep UI-only helpers out of `BeeCore/Func`.
- Keep transport/vendor helpers out of BeeInterface and BeeUi.
- Shared helpers should not depend on a higher layer just to reuse convenience
  code; move the helper to the lowest layer that both callers can reference.
