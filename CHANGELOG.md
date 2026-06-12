# Changelog — MechaBloom 3D

All notable changes to this project are documented here. Format loosely follows [Keep a Changelog](https://keepachangelog.com/). Dates are ISO (YYYY-MM-DD).

## [Unreleased]

### Added
- Project documentation set: `README.md`, `MEMORY_BANK.md`, `DEVELOPMENT_LOG.md`, `LICENSE_NOTES.md`, `ATTRIBUTION.md`, `CHANGELOG.md`.
- Verified private GitHub repository and Unity `.gitignore`.
- Core C# script foundation for managers, interactables, level configs, flow visuals, save/stars, hint/tutorial, audio, safe area, and camera shake.
- Editor-assembled `GameScene.unity` with the required manual hierarchy, 12 level parents, 12 level buttons, UI panels, managers, audio source placeholders, materials, camera, light, and primitive environment.
- Architecture-only script pass with serialized references and compile-safe placeholders for later gameplay work.
- Primitive gameplay prefab set: `Gear`, `Valve`, `Pipe`, `WaterSource`, `EnergyCore`, `Splitter`, `Blocker`, `PlantBed`, `BrokenGear`, and `LockedRoot`.
- Requested gameplay materials for gears, pipes, valves, water, energy, plant beds, cores, blockers, and splitters.

### Changed
- Advanced the project from documentation-only foundation to a compiled Unity scene/script foundation.
- Deferred early gameplay behavior from flow, plant, gear, valve, touch action, and star systems so the current code remains architecture-only.

### Notes
- Environment verified: Unity 6000.4.6f1 (URP), GitHub CLI authenticated, repo confirmed PRIVATE.
- Gameplay systems are not implemented yet; routed flow, object state changes, undo restoration, audio clips, and final level tuning remain.
- Prefabs use Unity primitive meshes only; no paid assets or external models are used.

---

## [0.1.0] — 2026-06-12
### Added
- Initial Unity project (URP template) and private repository setup.
