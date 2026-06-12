# MEMORY_BANK.md — MechaBloom 3D

> Living reference. Updated after every important change. Last updated: **2026-06-12**.

---

## 1. Game Identity

- **Title:** MechaBloom 3D (use this exact name everywhere — UI, menus, credits, docs, comments).
- **Genre:** 3D puzzle / mechanical-logic simulation.
- **Pitch:** A cosy-futuristic mechanical garden where plants bloom only when gears, valves, pipes, splitters, and energy cores route flow correctly to plant beds.
- **App Store:** Games → Puzzle (primary), Simulation (secondary).
- **Platforms:** Android + iPhone, Landscape, fully offline, no ads, no IAP.
- **Engine:** Unity 6000.4.6f1, URP, Linear color, Input System 1.19.0, TextMeshPro.

## 2. Core Gameplay Loop

Study board → select gear/valve/core → rotate/activate (costs an action) → flow recalculates from sources → flow travels through pipes/gears/valves/splitters, stops at blockers → correct flow blooms plant beds → manage limited actions + energy → all required beds bloomed = win → stars awarded.

## 3. Apple App Store Design-Spam Avoidance Strategy

The game must read as original, content-rich, and polished — **not** a hyper-casual tap loop, quiz, match clone, runner, or reskin. Achieved via:
- A genuine system (flow simulation through interacting mechanical parts), not a one-screen loop.
- 12 hand-designed levels that each introduce a distinct mechanic (gears → valves → blockers → splitters → energy → colour matching → action limits → broken gears → locked roots → mixed → final).
- Progression, star ratings, hints, undo, save system, replay value.
- Polished presentation through lighting, materials, animation, and particles on simple shapes.
- No deceptive mechanics, no manipulative monetisation.

## 4. Manual Scene Hierarchy Rules

The entire `GameScene` is **assembled by hand in the Editor**. Before pressing Play the developer can see: full environment, all UI panels, Main Camera, Audio Listener, Directional Light, EventSystem, all managers, all 12 level parents and their objects, all decorative + audio objects. Nothing core is built at runtime. See `DEVELOPMENT_LOG.md` for the authoritative current hierarchy and `README.md` for the target tree.

## 5. Runtime Creation Restrictions

Scripts **may not** create: UI, Canvas, buttons, text, sliders, panels, EventSystem, Camera, Audio Listener, AudioSources, environment, level layouts, gameplay objects, or any core object (no `CreatePrimitive`/`Instantiate` for core objects, no procedural scene builder in `Start`/`Awake`).

Scripts **may**: show/hide existing panels; update existing TMP text/sliders/images/bars/icons; enable/disable existing level parents and flow visuals; rotate existing gears/valves; animate existing objects; swap materials on existing renderers; reset existing level objects; play existing particle systems and audio sources; apply small optional camera shake on the existing Main Camera; save/load via PlayerPrefs.

## 6. Camera Rules

One manually placed **Main Camera**: Orthographic, isometric view, Position (8, 12, -8), Rotation (55, 45, 0), Orthographic Size tuned in Inspector. Holds the scene's **only Audio Listener**. No camera creation/move/follow/pan/zoom in code; no hardcoded transform; no reset in Start/Awake. Only `CameraShakeOnly` is allowed — it stores the Inspector position on Start, applies a small temporary shake on wrong action / level complete, then returns to origin without changing base angle or zoom.

## 7. UI Rules

All UI is hand-built under `UI > Canvas`. Canvas: Screen Space Overlay, Canvas Scaler = Scale With Screen Size, reference 1920×1080, Match 0.5, Graphic Raycaster on. EventSystem exists manually. `SafeAreaHandler` keeps key UI clear of notches/cutouts/home indicator. No runtime UI creation; no hardcoded layout positions. Buttons large and readable for mobile.

## 8. Audio and Licence Rules

`Audio` parent holds `MusicSource`, `SFXSource`, `UISFXSource` (created manually). `AudioManager` references them + AudioClips via `[SerializeField]`; never creates AudioSources at runtime; separates music/SFX volume; saves volume in PlayerPrefs. Only commercial-free audio (CC0 / Kenney / OpenGameArt CC0 / Freesound CC0 / self-made placeholders). Missing real audio → named placeholder slots documented in `LICENSE_NOTES.md`. All assets tracked in `LICENSE_NOTES.md` / `ATTRIBUTION.md`.

## 9. Level Design Rules

12 manually built levels under `Levels`, each a `Level_XX` parent with subgroups Grid/Gears/Pipes/Valves/EnergyCores/WaterSources/PlantBeds/Splitters/Blockers/FlowVisuals/LevelEffects and a `LevelConfig` component. Each level introduces one new idea (see §13 progression). No level geometry generated at runtime — `LevelManager` only enables/disables and resets existing parents.

## 10. Script Architecture

Clean modular C# in `Assets/Scripts/`. `[SerializeField] private` for scene refs; avoid public mutable fields, `GameObject.Find`, `FindObjectOfType`, `Resources.Load` for core objects. Public methods only where UI OnClick needs them. Required scripts: GameManager, LevelManager, LevelConfig, GardenGridManager, GardenTile, FlowPathCalculator, FlowVisualController, WaterSource, EnergyCore, GearController, PipeSegment, ValveController, SplitterController, BlockerTile, PlantBed, BrokenGear, LockedRoot, TouchInputController, InteractableObject, UIManager, LevelSelectUI, AudioManager, SaveManager, TutorialManager, StarRatingManager, HintManager, UndoManager, SafeAreaHandler, CameraShakeOnly, FeedbackTextUI.

## 11. Mobile Control Rules

`TouchInputController` raycasts from the existing Main Camera; works on Android + iOS; selects interactables (all have colliders); shows `SelectionRing` on selection; invalid taps give feedback. On-screen `MobileControlsPanel` buttons: Rotate, Activate, Undo, Restart, Pause, Hint — wired to public methods via Inspector. Editor shortcuts R/A/U/Esc for testing. Touch is primary.

## 12. Save System Rules

PlayerPrefs keys: highest unlocked level, stars per level, music volume, SFX volume, vibration placeholder, tutorial-seen flag. `SaveManager` is the single owner of these keys.

## 13. Level Progression (design intent)

01 tutorial (source→pipe→bed) · 02 gear rotation · 03 valve · 04 blocker/redirect · 05 splitter (bloom two) · 06 energy core/energy plant · 07 colour+flow matching · 08 limited actions · 09 broken gear (limited rotations) · 10 locked root (activate key first) · 11 mixed challenge · 12 final large multi-system board.

## 14. Star Rating Rules

3★ solved under target actions, no wrong actions, good energy remaining, (no hint if rule enabled). 2★ average actions / hint used / minor wrong actions. 1★ high actions or low remaining energy. Using a hint caps max rating at 2★.

## 15. Testing Checklist

See the full checklist in this file's companion section below and `DEVELOPMENT_LOG.md`. Key gates: project opens, no compile errors, no missing script refs, exactly one active Audio Listener, no runtime generation of scene/UI/camera/listener, 12 level buttons + 12 level parents exist manually, gameplay (gears/valves/flow/bloom/undo/hint/restart/pause/settings/audio/save/stars/win/lose) works, mobile UI fits landscape with safe area.

---

## CURRENT PROJECT STATUS

**Phase:** Foundation. Repo + documentation established.

- [x] Private GitHub repo `mhnmughal/MechaBloom-3D` (verified PRIVATE).
- [x] Unity project present (6000.4.6f1, URP), Unity .gitignore in place.
- [x] Documentation files created (README, MEMORY_BANK, DEVELOPMENT_LOG, LICENSE_NOTES, ATTRIBUTION, CHANGELOG).
- [ ] C# gameplay scripts.
- [ ] Materials + scene foundation (camera, light, environment, audio, managers).
- [ ] Manual UI canvas + panels.
- [ ] 12 manual levels.
- [ ] Reference wiring, audio placeholders, QA.

## KNOWN ISSUES

- Scene `GameScene.unity` not yet created (current active scene is the template `SampleScene`).
- No gameplay scripts compiled yet.

## NEXT TASKS

1. Author all C# scripts under `Assets/Scripts/` and confirm zero compile errors.
2. Create materials and the scene foundation (Main Camera, Directional Light, Environment, Audio, Managers, Effects, SharedGameplayObjects).
3. Build the manual UI canvas and all panels with SafeAreaHandler.
4. Build the 12 manual levels with LevelConfig.
5. Wire serialized references + OnClick events, add audio placeholders, run the QA checklist.
