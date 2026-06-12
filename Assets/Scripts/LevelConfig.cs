using TMPro;
using UnityEngine;

namespace MechaBloom
{
    public sealed class LevelConfig : MonoBehaviour
    {
        [SerializeField] private int levelNumber = 1;
        [SerializeField] private string levelName = "Level 01";
        [SerializeField] private string objectiveText = "Bloom every required plant bed.";
        [SerializeField] private int actionLimit = 12;
        [SerializeField] private int energyBudget = 10;
        [SerializeField] private int requiredBloomCount = 1;
        [SerializeField] private int star3ActionLimit = 4;
        [SerializeField] private int star2ActionLimit = 8;
        [SerializeField] private int star1ActionLimit = 12;
        [SerializeField] private string hintText = "Follow the flow from the source to the plant bed.";
        [SerializeField] private string[] tutorialMessages;
        [SerializeField] private WaterSource[] waterSources;
        [SerializeField] private EnergyCore[] energyCores;
        [SerializeField] private GearController[] gears;
        [SerializeField] private PipeSegment[] pipes;
        [SerializeField] private ValveController[] valves;
        [SerializeField] private SplitterController[] splitters;
        [SerializeField] private BlockerTile[] blockers;
        [SerializeField] private PlantBed[] plantBeds;
        [SerializeField] private FlowVisualController[] flowVisuals;
        [SerializeField] private ParticleSystem[] levelEffects;

        public int LevelNumber => levelNumber;
        public string LevelName => levelName;
        public string ObjectiveText => objectiveText;
        public int ActionLimit => actionLimit;
        public int EnergyBudget => energyBudget;
        public int RequiredBloomCount => requiredBloomCount;
        public int Star3ActionLimit => star3ActionLimit;
        public int Star2ActionLimit => star2ActionLimit;
        public int Star1ActionLimit => star1ActionLimit;
        public string HintText => hintText;
        public string[] TutorialMessages => tutorialMessages;
        public WaterSource[] WaterSources => waterSources;
        public EnergyCore[] EnergyCores => energyCores;
        public GearController[] Gears => gears;
        public PipeSegment[] Pipes => pipes;
        public ValveController[] Valves => valves;
        public SplitterController[] Splitters => splitters;
        public BlockerTile[] Blockers => blockers;
        public PlantBed[] PlantBeds => plantBeds;
        public FlowVisualController[] FlowVisuals => flowVisuals;
        public ParticleSystem[] LevelEffects => levelEffects;
    }
}
