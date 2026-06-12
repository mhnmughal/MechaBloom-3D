using UnityEngine;

namespace MechaBloom
{
    public sealed class FlowPathCalculator : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;

        public void Recalculate()
        {
            var level = levelManager != null ? levelManager.ActiveLevel : null;
            if (level == null)
            {
                return;
            }

            ResetVisuals(level);
            BloomReachableBeds(level, FlowType.Water);
            BloomReachableBeds(level, FlowType.Energy);
            levelManager.RefreshObjectiveState();
        }

        private static void ResetVisuals(LevelConfig level)
        {
            foreach (var pipe in level.Pipes)
            {
                if (pipe != null)
                {
                    pipe.SetFlowActive(false);
                }
            }

            foreach (var visual in level.FlowVisuals)
            {
                if (visual != null)
                {
                    visual.SetActive(false);
                }
            }
        }

        private static void BloomReachableBeds(LevelConfig level, FlowType flowType)
        {
            var sourceAvailable = flowType == FlowType.Water && HasWaterSource(level);
            sourceAvailable |= flowType == FlowType.Energy && HasActiveEnergyCore(level);
            if (!sourceAvailable)
            {
                return;
            }

            foreach (var pipe in level.Pipes)
            {
                if (pipe != null && (pipe.AcceptedFlow == flowType || pipe.AcceptedFlow == FlowType.None))
                {
                    pipe.SetFlowActive(true);
                }
            }

            foreach (var plantBed in level.PlantBeds)
            {
                if (plantBed != null)
                {
                    plantBed.TryReceiveFlow(flowType);
                }
            }

            foreach (var visual in level.FlowVisuals)
            {
                if (visual != null)
                {
                    visual.SetActive(true);
                }
            }
        }

        private static bool HasWaterSource(LevelConfig level)
        {
            foreach (var source in level.WaterSources)
            {
                if (source != null && source.Active)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasActiveEnergyCore(LevelConfig level)
        {
            foreach (var core in level.EnergyCores)
            {
                if (core != null && core.IsActive)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
