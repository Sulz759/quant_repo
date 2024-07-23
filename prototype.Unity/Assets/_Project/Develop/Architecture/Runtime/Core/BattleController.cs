using _Project.Develop.Architecture.Runtime.Core.Character;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleController: ILoadUnit
    {
        public CharacterView Bot { get; private set; }
        public CharacterView Player { get; private set; }

        private CharacterView _bot;
        
        private readonly CharacterFactory _characterFactory;
        
        public BattleController(CharacterFactory characterFactory)
        {
            _characterFactory = characterFactory;
        }
        public UniTask Load()
        {
            Player = _characterFactory.CreatePlayer();
            Bot = _characterFactory.CreateBot();
            
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