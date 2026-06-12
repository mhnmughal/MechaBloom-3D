# MechaBloom 3D

> Rotate gears. Guide flow. Bloom the garden.

**MechaBloom 3D** is a clean cosy-futuristic **3D mechanical garden puzzle** built in Unity using only default primitives. Plants don't grow from watering — they bloom when you align mechanical gears, open valves, route water and energy through pipes, and power plant beds correctly. Every level is a small 3D board you solve through planning, not reflexes.

---

## Game Identity

| | |
|---|---|
| **Title** | MechaBloom 3D |
| **Genre** | 3D puzzle / mechanical-logic simulation |
| **App Store Category** | Games → Puzzle (primary), Simulation (secondary) |
| **Platforms** | Android, iPhone (iOS) |
| **Orientation** | Landscape |
| **Monetisation** | None — no ads, no IAP |
| **Connectivity** | Fully offline |
| **Engine** | Unity 6000.4.6f1, Universal Render Pipeline (URP) |

## Core Gameplay Loop

1. Study the mechanical garden board.
2. Tap a gear, valve, or energy core to select it.
3. Rotate or activate the selected object (costs one action).
4. Flow recalculates from water sources and energy cores.
5. Water / energy travels tile-by-tile through pipes, gears, valves, and splitters.
6. Correct flow reaching a plant bed makes it bloom.
7. Manage a limited action and energy budget.
8. Bloom all required plant beds → level complete.
9. Earn 1–3 stars based on actions used, energy remaining, wrong moves, and hint usage.

## Content

- 12 hand-built, progressively complex levels (tutorial → final multi-system challenge)
- Main menu, level select, tutorial, gameplay HUD, settings, credits, pause, level-complete and game-over panels
- Save progress, star ratings, hint system, undo system
- Mobile touch controls with safe-area support

## Build & Run

1. Open the project in **Unity 6000.4.6f1** (or matching 6000.4.x).
2. Open `Assets/Scenes/GameScene.unity`.
3. Press **Play** in the Editor, or build for Android / iOS (Landscape).

> The full game scene is **manually assembled** in the Editor. Nothing core is generated at runtime — every camera, light, UI panel, manager, and level object exists in the hierarchy before pressing Play.

## Editor Test Shortcuts

| Key | Action |
|-----|--------|
| `R` | Rotate selected object |
| `A` | Activate selected object |
| `U` | Undo last action |
| `Esc` | Pause |

## Project Documentation

| File | Purpose |
|------|---------|
| `MEMORY_BANK.md` | Living design/architecture/status reference |
| `DEVELOPMENT_LOG.md` | Chronological change log |
| `CHANGELOG.md` | Versioned release notes |
| `LICENSE_NOTES.md` | Every external asset and its licence |
| `ATTRIBUTION.md` | Required attribution text (if any) |

## Asset & Licence Policy

Built with Unity default primitives. All fonts, sounds, music, icons, textures, and UI assets are placeholders or free for commercial use. **No paid or copyrighted assets are used.** See `LICENSE_NOTES.md` and `ATTRIBUTION.md`.

## License

Source code: see repository. Asset licensing is documented in `LICENSE_NOTES.md`.
