using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Checkpoint;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class GlobalMapController: ILoadUnit<GlobalMapConfiguration>
    {
        public IReadOnlyList<CheckpointView> Checkpoints => _checkpoints;

        private List<CheckpointView> _checkpoints;
        private readonly CheckpointFactory _checkpointFactory;

        public GlobalMapController(CheckpointFactory checkpointFactory)
        {
            _checkpointFactory = checkpointFactory;
        }
        public UniTask Load(GlobalMapConfiguration globalMapConfiguration)
        {

            _checkpoints = new List<CheckpointView>(globalMapConfiguration.CheckpointsCount);
            
            for (var i = 0; i < globalMapConfiguration.CheckpointsCount; i++) {
                var checkpoint = _checkpointFactory.CreateCheckpoint(new Vector3(i,i,i));
                
                //bot.transform.position = Random.insideUnitSphere * 15 + Player.transform.forward * 50 + Vector3.up * 15;
                checkpoint.gameObject.SetActive(false);
                _checkpoints.Add(checkpoint);
            }
            Log.Meta.D($"Meta is loading");
            return UniTask.CompletedTask;
        }

        public void StartMetaGame()
        {
            foreach (var checkpoint in _checkpoints)
            {
                checkpoint.gameObject.SetActive(true);
            }
            
            Log.Meta.D($"Start Meta Game");
        }
    }

    // TODO: Move to config file
    public readonly struct GlobalMapConfiguration
    {
        public readonly int CheckpointsCount;

        public GlobalMapConfiguration(int checkpointsCount)
        {
            CheckpointsCount = checkpointsCount;
        }
    }
}