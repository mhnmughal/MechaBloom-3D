# LICENSE_NOTES.md - MechaBloom 3D

This file documents every external asset (font, sound, music, icon, UI sprite, texture, model) used in the project, with source, author, licence, commercial-use status, and whether attribution is required.

> **Current status (2026-06-18):** No external paid or copyrighted assets are used. The project uses **Unity primitives**, the **TextMeshPro default font**, simple **self-authored materials**, and **self-authored placeholder audio WAVs**. No assets requiring attribution are present.

No external paid or copyrighted assets are used. Current project uses Unity primitives and placeholder audio only.

---

## Fonts

| Asset | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-------|--------|--------|---------|----------------|----------------------|-------|
| TextMeshPro default font (LiberationSans SDF) | Bundled with Unity TextMeshPro package | Unity / Liberation Sans (Red Hat) | SIL Open Font License 1.1 | Yes | No (OFL) | Default TMP font; ships with the project. |

If a custom font (Inter / Nunito / Poppins - all SIL OFL) is added later, record its exact file, source URL, version, and OFL copyright line here.

## Audio (SFX)

All committed SFX are self-authored/generated placeholder WAV tones created locally for MechaBloom 3D. They are project-owned placeholders, free for commercial use, and require no attribution.

| Slot name | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-----------|--------|--------|---------|----------------|----------------------|-------|
| `SFX_UIButtonClick.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | UI button click. |
| `SFX_ObjectSelect.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Object selection. |
| `SFX_GearRotate.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Gear rotation. |
| `SFX_ValveOpen.wav` / `SFX_ValveClose.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Valve actions. |
| `SFX_WaterFlowStart.wav` / `SFX_EnergyFlowStart.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Flow start cues. |
| `SFX_PlantSprout.wav` / `SFX_PlantBloom.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Plant growth/bloom cues. |
| `SFX_WrongAction.wav` / `SFX_BlockedFlow.wav` / `SFX_NotEnoughEnergy.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Error and blocked-flow cues. |
| `SFX_Undo.wav` / `SFX_Hint.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Utility cues. |
| `SFX_LevelComplete.wav` / `SFX_GameOver.wav` / `SFX_StarReward.wav` | Self-authored generated tone | Project developer | Project-owned placeholder | Yes | No | Completion/failure/reward cues. |

## Music

| Slot name | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-----------|--------|--------|---------|----------------|----------------------|-------|
| `Music_MechaBloom_SelfAuthoredPlaceholder.wav` | Self-authored generated loop | Project developer | Project-owned placeholder | Yes | No | Calm placeholder loop wired to `MusicSource`; replace or polish before release if desired. |

## Textures / Sprites / Icons

| Asset | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-------|--------|--------|---------|----------------|----------------------|-------|
| (none) | - | - | - | - | - | All visuals use Unity primitives + self-authored URP materials (solid colours/emission). No imported textures. |

## Materials

| Asset | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-------|--------|--------|---------|----------------|----------------------|-------|
| MechaBloom 3D solid-colour URP materials | Self-authored in Unity Editor | Project developer | Project-owned placeholder | Yes | No | Includes floor, tile, gear, pipe, valve, water, energy, plant, blocker, splitter, core, and UI panel materials. |
| Gameplay material set (`Gear_Brass`, `Gear_Selected`, `Pipe_Metal`, `Pipe_WaterActive`, `Pipe_Inactive`, `Valve_Open`, `Valve_Closed`, `Water_Blue`, `Energy_Green`, `Energy_Yellow`, `PlantBed_Empty`, `PlantBed_Growing`, `PlantBed_Bloomed`, `Core_Active`, `Core_Inactive`, `Blocker_Stone`, `Splitter_Metal`) | Self-authored in Unity Editor | Project developer | Project-owned placeholder | Yes | No | Solid-colour/emissive Unity materials only; no texture imports. |

## Models / Prefabs

| Asset | Source | Author | Licence | Notes |
|-------|--------|--------|---------|-------|
| Gameplay prefabs (`Gear`, `Valve`, `Pipe`, `WaterSource`, `EnergyCore`, `Splitter`, `Blocker`, `PlantBed`, `BrokenGear`, `LockedRoot`) | Self-authored in Unity Editor | Project developer | Project-owned placeholder | Built only from Unity primitive meshes (`Cube`, `Sphere`, `Cylinder`) plus project scripts/materials. No paid assets, external models, or imported meshes are used. |
| Environment primitives | Self-authored in Unity Editor | Project developer | Project-owned placeholder | Environment objects use Unity primitive meshes. |

---

## Approved free / CC0 sources for future audio

- Kenney.nl (CC0)
- OpenGameArt.org (filter to CC0)
- Freesound.org (filter to CC0 only)
- Self-generated placeholder tones

## Prohibited

Copyrighted music, random YouTube/internet audio, paid Asset Store audio, unclear-licence files, attribution-required audio unless documented here **and** in `ATTRIBUTION.md`.
