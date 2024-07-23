using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Checkpoint
{
    public class CheckpointView: MonoBehaviour
    {
        private CheckpointConfig _checkpointConfig;
        
        public void Initialize(CheckpointConfig checkpointConfig)
        {
            _checkpointConfig = checkpointConfig;
            //Log.Battle.D("Character is instantiate");
        }
    }
}