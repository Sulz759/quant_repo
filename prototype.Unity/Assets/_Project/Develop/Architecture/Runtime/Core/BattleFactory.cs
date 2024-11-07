using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Biome;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleFactory : ILoadUnit
    {
        private readonly CoreConfig _coreConfig;
        private readonly LevelDataStorage _levelDataStorage;
        private readonly TouchscreenInput _touchscreen;

        private TrainView _trainPrefab;
        private BiomeView _biomePrefab;
        private RailwayView _railwayPrefab;
        
        public BattleFactory(LevelDataStorage levelDataStorage,CoreConfig coreConfig, TouchscreenInput touchscreen)
        {
            _levelDataStorage = levelDataStorage;
            _coreConfig = coreConfig;
            _touchscreen = touchscreen;
        }

        public UniTask Load()
        {
            var data = _levelDataStorage.GetLevelData(_coreConfig.Container.levelNumber);
            
            _trainPrefab = Resources.Load<TrainView>("Prefabs/Core/Train/Train"); // to do trainFactory
            _biomePrefab = Resources.Load<BiomeView>("Prefabs/Core/Bioms/" + $"{data.biomeName}");
            _railwayPrefab = Resources.Load<RailwayView>("Prefabs/Core/Railway/Railway");

            return UniTask.CompletedTask;
        }
        
        public BiomeView CreateBiome()
        {
            return Object.Instantiate(_biomePrefab);
        }

        public RailwayView CreateRailway()
        {
            return Object.Instantiate(_railwayPrefab);
        }

        public TrainView CreateTrain(List<WayView> ways)
        {
            var player = Object.Instantiate(_trainPrefab, ways[1].transform.position, Quaternion.identity);
            player.Initialize(_coreConfig.Container.TrainConfig, _touchscreen, ways);
            return player;
        }
    }
}