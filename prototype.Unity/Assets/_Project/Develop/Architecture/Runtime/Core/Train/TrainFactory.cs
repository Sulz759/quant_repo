using _Project.Develop.Architecture.Runtime.Core.Biome;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainFactory : ILoadUnit
    {
        private readonly ConfigContainer _configs;
        private readonly TouchscreenInput _touchscreen;

        private TrainView _trainPrefab;
        private BiomeView _biomePrefab;

        //private TrainView _playerTrain;
        public TrainFactory(ConfigContainer configs, TouchscreenInput touchscreen)
        {
            _configs = configs;
            _touchscreen = touchscreen;
        }

        public UniTask Load()
        {
            _trainPrefab = Resources.Load<TrainView>("Prefabs/Core/Train");
            _biomePrefab = Resources.Load<BiomeView>("Prefabs/Core/Bioms/" + $"{_configs.Container.biomeName}");

            return UniTask.CompletedTask;
        }
        
        public BiomeView CreateBiome()
        {
            return Object.Instantiate(_biomePrefab);
        }
        

        public TrainView CreateTrain()
        {
            var player = Object.Instantiate(_trainPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            player.Initialize(_configs.Container.TrainConfig, _touchscreen);
            return player;
        }
    }
}