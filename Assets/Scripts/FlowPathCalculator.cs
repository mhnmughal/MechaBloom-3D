using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MechaBloom
{
    public sealed class FlowPathCalculator : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private GardenGridManager gardenGridManager;
        [SerializeField] private AudioManager audioManager;

        public LevelManager LevelManager => levelManager;
        public GardenGridManager GardenGridManager => gardenGridManager;
        public int LastBloomedCount { get; private set; }
        public bool LastHadBlockedFlow { get; private set; }
        public bool LastHadWaterFlow { get; private set; }
        public bool LastHadEnergyFlow { get; private set; }

        private readonly Dictionary<Vector2Int, PipeSegment> pipes = new();
        private readonly Dictionary<Vector2Int, ValveController> valves = new();
        private readonly Dictionary<Vector2Int, SplitterController> splitters = new();
        private readonly Dictionary<Vector2Int, PlantBed> plantBeds = new();
        private readonly Dictionary<Vector2Int, GearController> gears = new();
        private readonly HashSet<Vector2Int> blockers = new();
        private readonly Dictionary<Vector2Int, LockedRoot> lockedRoots = new();
        private readonly HashSet<string> visited = new();

        public int Recalculate(bool evaluateOutcome = true)
        {
            var level = levelManager != null ? levelManager.ActiveLevel : null;
            if (level == null)
            {
                LastBloomedCount = 0;
                return 0;
            }

            BuildLookup(level);
            ResetDynamicObjects(level, true);
            TraverseLevel(level);

            if (AnyEnergyPlantBloomed(level) && UnlockRoots(level))
            {
                ResetDynamicObjects(level, false);
                TraverseLevel(level);
            }

            LastBloomedCount = CountBloomed(level);
            if (LastHadWaterFlow)
            {
                audioManager?.PlayWaterFlowStart();
            }

            if (LastHadEnergyFlow)
            {
                audioManager?.PlayEnergyFlowStart();
            }

            if (LastHadBlockedFlow)
            {
                audioManager?.PlayBlockedFlow();
            }

            if (evaluateOutcome)
            {
                levelManager?.EvaluateLevelState();
            }

            return LastBloomedCount;
        }

        private void BuildLookup(LevelConfig level)
        {
            pipes.Clear();
            valves.Clear();
            splitters.Clear();
            plantBeds.Clear();
            gears.Clear();
            blockers.Clear();
            lockedRoots.Clear();

            gardenGridManager?.UseLevel(level);

            foreach (var pipe in Safe(level.Pipes))
            {
                AddByTile(pipes, ResolveTile(pipe.Tile, pipe.transform.position), pipe);
            }

            foreach (var valve in Safe(level.Valves))
            {
                var tile = ResolveInteractableTile(valve);
                AddByTile(valves, tile, valve);
            }

            foreach (var splitter in Safe(level.Splitters))
            {
                AddByTile(splitters, ResolveTile(splitter.Tile, splitter.transform.position), splitter);
            }

            foreach (var bed in Safe(level.PlantBeds))
            {
                AddByTile(plantBeds, ResolveTile(bed.Tile, bed.transform.position), bed);
            }

            foreach (var gear in Safe(level.Gears))
            {
                var tile = ResolveInteractableTile(gear);
                AddByTile(gears, tile, gear);
            }

            foreach (var blocker in Safe(level.Blockers))
            {
                var tile = ResolveTile(blocker.Tile, blocker.transform.position);
                if (tile != null)
                {
                    blockers.Add(tile.GridPosition);
                }
            }

            foreach (var root in level.GetComponentsInChildren<LockedRoot>(true))
            {
                var tile = ResolveTile(null, root.transform.position);
                if (tile != null && !lockedRoots.ContainsKey(tile.GridPosition))
                {
                    lockedRoots.Add(tile.GridPosition, root);
                }
            }
        }

        private static IEnumerable<T> Safe<T>(T[] items) where T : Object
        {
            return items == null ? Enumerable.Empty<T>() : items.Where(item => item != null);
        }

        private void AddByTile<T>(Dictionary<Vector2Int, T> lookup, GardenTile tile, T item)
        {
            if (tile == null || lookup.ContainsKey(tile.GridPosition))
            {
                return;
            }

            lookup.Add(tile.GridPosition, item);
        }

        private GardenTile ResolveInteractableTile(InteractableObject interactable)
        {
            var tile = ResolveTile(interactable.Tile, interactable.transform.position);
            if (tile != null)
            {
                interactable.SetRuntimeTile(tile);
            }

            return tile;
        }

        private GardenTile ResolveTile(GardenTile assignedTile, Vector3 position)
        {
            return assignedTile != null ? assignedTile : gardenGridManager != null ? gardenGridManager.GetNearestTile(position) : null;
        }

        private void ResetDynamicObjects(LevelConfig level, bool resetRoots)
        {
            LastHadBlockedFlow = false;
            LastHadWaterFlow = false;
            LastHadEnergyFlow = false;

            foreach (var pipe in Safe(level.Pipes))
            {
                pipe.SetFlowActive(false);
            }

            foreach (var bed in Safe(level.PlantBeds))
            {
                bed.ResetState();
            }

            foreach (var visual in Safe(level.FlowVisuals))
            {
                visual.SetActive(false);
            }

            if (!resetRoots)
            {
                return;
            }

            foreach (var root in level.GetComponentsInChildren<LockedRoot>(true))
            {
                root.ResetState();
            }
        }

        private void TraverseLevel(LevelConfig level)
        {
            visited.Clear();

            foreach (var source in Safe(level.WaterSources))
            {
                var tile = ResolveTile(source.Tile, source.transform.position);
                if (tile != null)
                {
                    source.SetRuntimeTile(tile);
                }

                if (!source.Active || tile == null)
                {
                    continue;
                }

                LastHadWaterFlow = true;
                FlowFrom(tile.GridPosition, source.OutputDirection, FlowType.Water);
            }

            foreach (var core in Safe(level.EnergyCores))
            {
                if (!core.Active)
                {
                    continue;
                }

                var tile = ResolveInteractableTile(core);
                if (tile == null)
                {
                    continue;
                }

                LastHadEnergyFlow = true;
                foreach (GridDirection direction in System.Enum.GetValues(typeof(GridDirection)))
                {
                    FlowFrom(tile.GridPosition, direction, FlowType.Energy);
                }
            }

            foreach (var visual in Safe(level.FlowVisuals))
            {
                visual.SetActive(LastHadWaterFlow || LastHadEnergyFlow);
            }
        }

        private void FlowFrom(Vector2Int currentPosition, GridDirection direction, FlowType flowType)
        {
            var nextPosition = currentPosition + GridDirectionUtility.ToOffset(direction);
            if (gardenGridManager == null || !gardenGridManager.TryGetTile(nextPosition, out var tile))
            {
                return;
            }

            var enterDirection = GridDirectionUtility.Opposite(direction);
            VisitTile(tile, enterDirection, flowType);
        }

        private void VisitTile(GardenTile tile, GridDirection enteredFrom, FlowType flowType)
        {
            if (tile == null)
            {
                return;
            }

            var key = $"{tile.GridPosition.x}:{tile.GridPosition.y}:{enteredFrom}:{flowType}";
            if (!visited.Add(key))
            {
                return;
            }

            if (IsBlocked(tile.GridPosition))
            {
                LastHadBlockedFlow = true;
                return;
            }

            var occupied = false;
            if (pipes.TryGetValue(tile.GridPosition, out var pipe))
            {
                occupied = true;
                if (pipe.AcceptedFlow != flowType)
                {
                    LastHadBlockedFlow = true;
                    return;
                }

                pipe.SetFlowActive(true);
            }

            if (plantBeds.TryGetValue(tile.GridPosition, out var bed))
            {
                occupied = true;
                var wasBloomed = bed.IsBloomed;
                if (!bed.TryReceiveFlow(flowType))
                {
                    LastHadBlockedFlow = true;
                    return;
                }

                if (!wasBloomed && bed.IsBloomed)
                {
                    audioManager?.PlayPlantBloom();
                }
            }

            var exits = GetOutgoingDirections(tile.GridPosition, enteredFrom, out var hasInteractiveConnector);
            occupied |= hasInteractiveConnector;
            foreach (var outgoing in exits)
            {
                FlowFrom(tile.GridPosition, outgoing, flowType);
            }

            if (!occupied)
            {
                LastHadBlockedFlow = true;
            }
        }

        private List<GridDirection> GetOutgoingDirections(Vector2Int position, GridDirection enteredFrom, out bool occupied)
        {
            var exits = new List<GridDirection>();
            occupied = false;

            if (valves.TryGetValue(position, out var valve))
            {
                occupied = true;
                if (!valve.IsOpen)
                {
                    LastHadBlockedFlow = true;
                    return exits;
                }

                foreach (GridDirection direction in System.Enum.GetValues(typeof(GridDirection)))
                {
                    if (direction != enteredFrom)
                    {
                        exits.Add(direction);
                    }
                }
            }

            if (splitters.TryGetValue(position, out var splitter))
            {
                occupied = true;
                if (splitter.OutputDirections != null && splitter.OutputDirections.Length > 0)
                {
                    foreach (var direction in splitter.OutputDirections)
                    {
                        if (direction != enteredFrom)
                        {
                            exits.Add(direction);
                        }
                    }
                }
            }

            if (gears.TryGetValue(position, out var gear))
            {
                occupied = true;
                foreach (var direction in GetGearExits(gear, enteredFrom))
                {
                    exits.Add(direction);
                }
            }

            if (pipes.ContainsKey(position) && !gears.ContainsKey(position) && !splitters.ContainsKey(position) && !valves.ContainsKey(position))
            {
                occupied = true;
                foreach (GridDirection direction in System.Enum.GetValues(typeof(GridDirection)))
                {
                    if (direction != enteredFrom)
                    {
                        exits.Add(direction);
                    }
                }
            }

            if (lockedRoots.TryGetValue(position, out var root) && !root.Locked)
            {
                occupied = true;
                foreach (GridDirection direction in System.Enum.GetValues(typeof(GridDirection)))
                {
                    if (direction != enteredFrom && !exits.Contains(direction))
                    {
                        exits.Add(direction);
                    }
                }
            }

            return exits;
        }

        private static IEnumerable<GridDirection> GetGearExits(GearController gear, GridDirection enteredFrom)
        {
            var a = (GridDirection)(gear.QuarterTurns % 4);
            var b = (GridDirection)(((int)a + 1) % 4);

            if (enteredFrom == a)
            {
                yield return b;
            }
            else if (enteredFrom == b)
            {
                yield return a;
            }
        }

        private bool IsBlocked(Vector2Int position)
        {
            if (blockers.Contains(position))
            {
                return true;
            }

            return lockedRoots.TryGetValue(position, out var root) && root.Locked;
        }

        private static bool AnyEnergyPlantBloomed(LevelConfig level)
        {
            return level.PlantBeds != null && level.PlantBeds.Any(bed => bed != null && bed.RequiredFlow == FlowType.Energy && bed.IsBloomed);
        }

        private static bool UnlockRoots(LevelConfig level)
        {
            var changed = false;
            foreach (var root in level.GetComponentsInChildren<LockedRoot>(true))
            {
                if (root.Locked)
                {
                    root.Unlock();
                    changed = true;
                }
            }

            return changed;
        }

        private static int CountBloomed(LevelConfig level)
        {
            return level.PlantBeds == null ? 0 : level.PlantBeds.Count(bed => bed != null && bed.IsBloomed);
        }
    }
}
