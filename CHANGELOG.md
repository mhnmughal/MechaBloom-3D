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
- Manual `Level_01` through `Level_06` scene layouts with LevelConfig objective, hint, tutorial text, and mechanic-specific objects.
- Manual `Level_07` through `Level_12` scene layouts for flow matching, limited actions, broken gear, locked root, mixed systems, and final challenge mechanics.
- Gameplay systems integration: water/energy flow traversal, splitter and blocker handling, plant blooming, locked root unlocks, win/loss, undo, hints, save/unlock, star ratings, touch/mobile input, UI updates, and audio hooks.
- Self-authored placeholder WAV audio for UI clicks, selection, gear/valve actions, water/energy flow, plants, wrong/blocked actions, undo, hint, level complete, game over, star reward, and background music.
- Persistent Inspector wiring for all manually placed UI buttons, settings sliders, vibration toggle, tutorial controls, mobile controls, level navigation, pause, retry, restart, and next-level actions.
- Play Mode solver verification for all 12 manually placed levels.

### Changed
- Advanced the project from documentation-only foundation to a compiled Unity scene/script foundation.
- Advanced the code from architecture-only placeholders to integrated gameplay behavior driven by existing scene objects.
- Tuned Levels 07-12, Level 05 splitter output, tile references, and locked-root pass-through behavior so all levels have valid solutions under the implemented flow rules.
- Set the project to landscape-only mobile orientation and enabled auto-sizing/wrapping for all TextMeshPro UI text.

### Notes
- Environment verified: Unity 6000.4.6f1 (URP), GitHub CLI authenticated, repo confirmed PRIVATE.
- Automated Play Mode smoke QA passes for all 12 levels; real-device Android/iPhone QA remains recommended before release.
- Prefabs use Unity primitive meshes only; no paid assets or external models are used.
- Levels 01-12 are present before Play and connected to gameplay systems; Levels 01, 04, and 05 are no-action tutorial/demo boards that auto-complete when already solved.

---

## [0.1.0] — 2026-06-12
### Added
- Initial Unity project (URP template) and private repository setup.
