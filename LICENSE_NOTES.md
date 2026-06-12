# LICENSE_NOTES.md — MechaBloom 3D

This file documents every external asset (font, sound, music, icon, UI sprite, texture, model) used in the project, with source, author, licence, commercial-use status, and whether attribution is required.

> **Current status (2026-06-12):** No external paid or copyrighted assets are used. The project uses **Unity primitives**, the **TextMeshPro default font**, simple **self-authored materials**, and **placeholder audio slots only**. No assets requiring attribution are present yet.

No external paid or copyrighted assets are used. Current project uses Unity primitives and placeholder audio only.

---

## Fonts

| Asset | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-------|--------|--------|---------|----------------|----------------------|-------|
| TextMeshPro default font (LiberationSans SDF) | Bundled with Unity TextMeshPro package | Unity / Liberation Sans (Red Hat) | SIL Open Font License 1.1 | Yes | No (OFL) | Default TMP font; ships with the project. |

If a custom font (Inter / Nunito / Poppins — all SIL OFL) is added later, record its exact file, source URL, version, and OFL copyright line here.

## Audio (SFX)

All SFX are **named placeholder slots** until verified CC0 files are added. No real audio files are committed yet.

| Slot name | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-----------|--------|--------|---------|----------------|----------------------|-------|
| UI button click | placeholder | — | TBD (CC0 target) | Yes (target) | No (CC0) | Replace with CC0 file. |
| Object select | placeholder | — | TBD (CC0 target) | Yes | No | |
| Gear rotate | placeholder | — | TBD (CC0 target) | Yes | No | |
| Valve open / close | placeholder | — | TBD (CC0 target) | Yes | No | |
| Water flow start | placeholder | — | TBD (CC0 target) | Yes | No | |
| Energy flow start | placeholder | — | TBD (CC0 target) | Yes | No | |
| Plant sprout / bloom | placeholder | — | TBD (CC0 target) | Yes | No | |
| Wrong action / blocked / not enough energy | placeholder | — | TBD (CC0 target) | Yes | No | |
| Undo / hint | placeholder | — | TBD (CC0 target) | Yes | No | |
| Level complete / game over / star reward | placeholder | — | TBD (CC0 target) | Yes | No | |

## Music

| Slot name | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-----------|--------|--------|---------|----------------|----------------------|-------|
| `Music_FreeLicensePlaceholder` (calm cosy mechanical garden loop) | placeholder | — | TBD (CC0 target) | Yes (target) | No (CC0 target) | Real CC0 music must be added later before publishing. |

## Textures / Sprites / Icons

| Asset | Source | Author | Licence | Commercial use | Attribution required | Notes |
|-------|--------|--------|---------|----------------|----------------------|-------|
| (none) | — | — | — | — | — | All visuals use Unity primitives + self-authored URP materials (solid colours/emission). No imported textures. |

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
