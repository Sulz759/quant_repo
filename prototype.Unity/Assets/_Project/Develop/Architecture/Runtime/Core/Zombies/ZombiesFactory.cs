using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Zombies
{
    public class ZombiesFactory: ILoadUnit
    {
        private readonly CoreConfig _coreConfig;
        private readonly LevelDataStorage _levelDataStorage;
        
        private List<GameObject> _zombies;

        public ZombiesFactory(LevelDataStorage levelDataStorage, CoreConfig coreConfig)
        {
            _levelDataStorage = levelDataStorage;
            _coreConfig = coreConfig;
        }
        
        public UniTask Load()
        {
            var data = _levelDataStorage.GetLevelData(_coreConfig.Container.levelNumber);

            Log.Battle.D($"{data.waves[0].direction}");
            
            return UniTask.CompletedTask;
        }
    }
}