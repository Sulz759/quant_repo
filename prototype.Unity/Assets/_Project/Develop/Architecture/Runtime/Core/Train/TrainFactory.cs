using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainFactory: ILoadUnit
    {
        private readonly ConfigContainer _configs;
        private TrainView _prefab;
        private TrainView _playerTrain;
        public TrainFactory(ConfigContainer configs)
        {
            _configs = configs;
        }
        public UniTask Load()
        {
            _prefab = Resources.Load<TrainView>("Prefabs/Core/Train");
            
            return UniTask.CompletedTask;
        }

        public TrainView CreateTrain()
        {
            var player = Object.Instantiate(_prefab, new Vector3(-6,0,-2.7f),Quaternion.identity);
            player.Initialize(_configs.Battle.TrainConfig, _configs.Battle.InputConfig);
            return player;
        }

        public TrainView CreateBot()
        {
            var bot = Object.Instantiate(_prefab, new Vector3(2,0,-2.7f),Quaternion.identity);
            var botConfig = _configs.Battle.BotTrainConfig;
            
            //bot.Initialize(botConfig);
            return bot;
        }
    }
}