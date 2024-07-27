using System;
using System.Collections.Generic;
using System.Numerics;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime
{
    public sealed class ConfigContainer : ILoadUnit
    {
        public BattleConfigContainer Battle;

        public UniTask Load()
        {
            // TODO: подгрузку конфигов продумать
            Battle = new BattleConfigContainer();

            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class BattleConfigContainer
    {
        public CharacterConfig CharacterConfig;
        public BotCharacterConfig BotCharacterConfig;
    }

    public class CharacterConfig { }

    public class BotCharacterConfig : CharacterConfig { }
    
}