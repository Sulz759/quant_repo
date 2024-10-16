using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleController: ILoadUnit
    {
        public TrainView Bot { get; private set; }
        public TrainView Player { get; private set; }

        private TrainView _bot;
        
        private readonly TrainFactory _trainFactory;
        
        public BattleController(TrainFactory trainFactory)
        {
            _trainFactory = trainFactory;
        }
        public UniTask Load()
        {
            Player = _trainFactory.CreateTrain();
            Bot = _trainFactory.CreateBot();
            
            Player.gameObject.SetActive(false);
            Bot.gameObject.SetActive(false);
            
            Log.Battle.D($"Battle is loading");

            return UniTask.CompletedTask;
        }

        public void StartBattle()
        {
            Player.gameObject.SetActive(true);
            Bot.gameObject.SetActive(true);
            
            Log.Battle.D($"Start battle");
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