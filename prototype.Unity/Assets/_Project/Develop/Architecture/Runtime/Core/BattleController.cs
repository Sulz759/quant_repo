using _Project.Develop.Architecture.Runtime.Core.Biome;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Core.Zombies;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleController : ILoadUnit
    {
        private readonly BattleFactory _battleFactory;
        public BattleController(BattleFactory battleFactory, ZombieSpawnController zombieSpawnController)
        {
            _battleFactory = battleFactory;
        }
        public BiomeView Biome { get; private set; }
        public RailwayView Railway { get; private set; }
        public TrainView Player { get; private set; }
        
        public ZombieSpawnController ZombieSpawner { get; private set; }

        public UniTask Load()
        {
            Biome = _battleFactory.CreateBiome();
            Railway = _battleFactory.CreateRailway();
            Player = _battleFactory.CreateTrain(Railway);
            
            ZombieSpawner = _battleFactory.GetZombieSpawner();

            Player.gameObject.SetActive(false);
            

            Log.Battle.D("Battle is loading");

            return UniTask.CompletedTask;
        }

        public void StartBattle()
        {
            Player.gameObject.SetActive(true);

            

            Log.Battle.D("Start battle");

            Log.Battle.D("Start zombie spawn" +
                         $" waves: {ZombieSpawner.Waves.Count}" +
                         $" first wave: {ZombieSpawner.Waves[0].direction}");
        }
    }
}