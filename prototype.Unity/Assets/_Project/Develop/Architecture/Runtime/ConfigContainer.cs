using System;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime
{
    public sealed class ConfigContainer : ILoadUnit
    {
        public BattleConfigContainer Battle;
        public GlobalMapConfigContainer GlobalMap;

        public UniTask Load()
        {
            // TODO: подгрузку конфигов продумать
            Battle = new BattleConfigContainer(); 
            GlobalMap = new GlobalMapConfigContainer();
            
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class BattleConfigContainer
    {
        public CharacterConfig CharacterConfig;
        public BotCharacterConfig BotCharacterConfig;
    }

    [Serializable]
    public class GlobalMapConfigContainer
    {
        public CheckpointConfig CheckpointConfig;
    }

    public class CharacterConfig { }

    public class BotCharacterConfig : CharacterConfig { }
    
    public class CheckpointConfig { }
}