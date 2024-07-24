using _Project.Develop.Architecture.Runtime.Meta.Checkpoint;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class CheckpointFactory: ILoadUnit
    {
        private readonly ConfigContainer _configs;
        private CheckpointView _prefab;

        public CheckpointFactory(ConfigContainer configs)
        {
            _configs = configs;
        }
        
        public UniTask Load()
        {
            _prefab = Resources.Load<CheckpointView>("Prefabs/Meta/Checkpoint");
            return UniTask.CompletedTask;
        }

        public CheckpointView CreateCheckpoint(Vector3 coords)
        {
            var checkpoint = Object.Instantiate(_prefab, coords,Quaternion.identity);
            
            checkpoint.Initialize(_configs.GlobalMap.CheckpointConfig);
            return checkpoint;
        }
    }
}