using _Project.Develop.Architecture.Runtime.Core.Biome;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleController : ILoadUnit
    {
        private readonly TrainFactory _trainFactory;
        public BattleController(TrainFactory trainFactory)
        {
            _trainFactory = trainFactory;
        }
        public BiomeView Biome { get; private set; }
        public RailwayView Railway { get; private set; }
        public TrainView Player { get; private set; }

        public UniTask Load()
        {
            this.Biome = _trainFactory.CreateBiome();
            this.Railway = _trainFactory.CreateRailway();
            this.Player = _trainFactory.CreateTrain();

            Player.gameObject.SetActive(false);
            

            Log.Battle.D("Battle is loading");

            return UniTask.CompletedTask;
        }

        public void StartBattle()
        {
            Player.gameObject.SetActive(true);

            Log.Battle.D("Start battle");
        }
    }

    // TODO: Move to config file
    public readonly struct LevelConfiguration
    {
        public readonly int EnemiesCount;

        public LevelConfiguration(int enemiesCount)
        {
            EnemiesCount = enemiesCount;
        }
    }
}