using System.Collections.Generic;
using UnityEngine;

namespace MechaBloom
{
    public sealed class UndoManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private FlowPathCalculator flowPathCalculator;
        [SerializeField] private FeedbackTextUI feedbackTextUI;
        [SerializeField] private AudioManager audioManager;

        private readonly Stack<LevelSnapshot> snapshots = new();

        public void UndoLastAction()
        {
            if (levelManager == null || snapshots.Count == 0)
            {
                feedbackTextUI?.Show("Nothing to undo.");
                return;
            }

            var snapshot = snapshots.Pop();
            snapshot.Restore();
            levelManager.RestoreRuntimeState(snapshot.ActionsUsed, snapshot.EnergyRemaining, snapshot.WrongActions, snapshot.HintUsed);
            flowPathCalculator?.Recalculate(false);
            feedbackTextUI?.Show("Undo");
            audioManager?.PlayUndo();
        }

        public void CaptureState()
        {
            if (levelManager == null || levelManager.ActiveLevel == null)
            {
                return;
            }

            snapshots.Push(LevelSnapshot.Capture(levelManager));
        }

        public void DiscardLatest()
        {
            if (snapshots.Count > 0)
            {
                snapshots.Pop();
            }
        }

        public void Clear()
        {
            snapshots.Clear();
        }

        private sealed class LevelSnapshot
        {
            private readonly GearState[] gears;
            private readonly ValveState[] valves;
            private readonly CoreState[] cores;
            private readonly PlantState[] plants;
            private readonly RootState[] roots;

            public int ActionsUsed { get; private set; }
            public int EnergyRemaining { get; private set; }
            public int WrongActions { get; private set; }
            public bool HintUsed { get; private set; }

            private LevelSnapshot(GearState[] gears, ValveState[] valves, CoreState[] cores, PlantState[] plants, RootState[] roots)
            {
                this.gears = gears;
                this.valves = valves;
                this.cores = cores;
                this.plants = plants;
                this.roots = roots;
            }

            public static LevelSnapshot Capture(LevelManager manager)
            {
                var level = manager.ActiveLevel;
                var snapshot = new LevelSnapshot(
                    CaptureGears(level),
                    CaptureValves(level),
                    CaptureCores(level),
                    CapturePlants(level),
                    CaptureRoots(level));

                snapshot.ActionsUsed = manager.ActionsUsed;
                snapshot.EnergyRemaining = manager.EnergyRemaining;
                snapshot.WrongActions = manager.WrongActions;
                snapshot.HintUsed = manager.HintUsed;
                return snapshot;
            }

            public void Restore()
            {
                foreach (var gear in gears)
                {
                    gear.Restore();
                }

                foreach (var valve in valves)
                {
                    valve.Restore();
                }

                foreach (var core in cores)
                {
                    core.Restore();
                }

                foreach (var plant in plants)
                {
                    plant.Restore();
                }

                foreach (var root in roots)
                {
                    root.Restore();
                }
            }

            private static GearState[] CaptureGears(LevelConfig level)
            {
                var gears = level.Gears ?? new GearController[0];
                var states = new GearState[gears.Length];
                for (var i = 0; i < gears.Length; i++)
                {
                    states[i] = new GearState(gears[i]);
                }

                return states;
            }

            private static ValveState[] CaptureValves(LevelConfig level)
            {
                var valves = level.Valves ?? new ValveController[0];
                var states = new ValveState[valves.Length];
                for (var i = 0; i < valves.Length; i++)
                {
                    states[i] = new ValveState(valves[i]);
                }

                return states;
            }

            private static CoreState[] CaptureCores(LevelConfig level)
            {
                var cores = level.EnergyCores ?? new EnergyCore[0];
                var states = new CoreState[cores.Length];
                for (var i = 0; i < cores.Length; i++)
                {
                    states[i] = new CoreState(cores[i]);
                }

                return states;
            }

            private static PlantState[] CapturePlants(LevelConfig level)
            {
                var plants = level.PlantBeds ?? new PlantBed[0];
                var states = new PlantState[plants.Length];
                for (var i = 0; i < plants.Length; i++)
                {
                    states[i] = new PlantState(plants[i]);
                }

                return states;
            }

            private static RootState[] CaptureRoots(LevelConfig level)
            {
                var roots = level.GetComponentsInChildren<LockedRoot>(true);
                var states = new RootState[roots.Length];
                for (var i = 0; i < roots.Length; i++)
                {
                    states[i] = new RootState(roots[i]);
                }

                return states;
            }
        }

        private readonly struct GearState
        {
            private readonly GearController gear;
            private readonly int quarterTurns;
            private readonly int rotationsUsed;

            public GearState(GearController gear)
            {
                this.gear = gear;
                quarterTurns = gear != null ? gear.QuarterTurns : 0;
                rotationsUsed = gear != null ? gear.RotationsUsed : 0;
            }

            public void Restore()
            {
                gear?.SetState(quarterTurns, rotationsUsed);
            }
        }

        private readonly struct ValveState
        {
            private readonly ValveController valve;
            private readonly bool open;

            public ValveState(ValveController valve)
            {
                this.valve = valve;
                open = valve != null && valve.IsOpen;
            }

            public void Restore()
            {
                valve?.SetOpen(open);
            }
        }

        private readonly struct CoreState
        {
            private readonly EnergyCore core;
            private readonly bool active;

            public CoreState(EnergyCore core)
            {
                this.core = core;
                active = core != null && core.Active;
            }

            public void Restore()
            {
                core?.SetActive(active);
            }
        }

        private readonly struct PlantState
        {
            private readonly PlantBed plant;
            private readonly PlantGrowthStage stage;

            public PlantState(PlantBed plant)
            {
                this.plant = plant;
                stage = plant != null ? plant.Stage : PlantGrowthStage.Empty;
            }

            public void Restore()
            {
                plant?.SetStage(stage);
            }
        }

        private readonly struct RootState
        {
            private readonly LockedRoot root;
            private readonly bool locked;

            public RootState(LockedRoot root)
            {
                this.root = root;
                locked = root != null && root.Locked;
            }

            public void Restore()
            {
                root?.SetLocked(locked);
            }
        }
    }
}
