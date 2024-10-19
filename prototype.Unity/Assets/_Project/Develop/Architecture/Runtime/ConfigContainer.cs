using System;
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
        public BotTrainConfig BotTrainConfig;
        public TrainConfig TrainConfig;
    }

    public class TrainConfig
    {
    }

    public class BotTrainConfig : TrainConfig
    {
    }
}