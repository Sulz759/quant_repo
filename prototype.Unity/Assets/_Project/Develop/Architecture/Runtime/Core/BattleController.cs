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
        private readonly ZombiesFactory _zombiesFactory;
        public BattleController(BattleFactory battleFactory, ZombiesFactory zombiesFactory)
        {
            _battleFactory = battleFactory;
            _zombiesFactory = zombiesFactory;
        }
        public BiomeView Biome { get; private set; }
        public RailwayView Railway { get; private set; }
        public TrainView Player { get; private set; }

        public UniTask Load()
        {
            this.Biome = _battleFactory.CreateBiome();
            this.Railway = _battleFactory.CreateRailway();
            this.Player = _battleFactory.CreateTrain(Railway.GetWays());

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
}