using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeView: MonoBehaviour
    {
        private NodeConfig _nodeConfig;
        
        public void Initialize(NodeConfig nodeConfig)
        {
            _nodeConfig = nodeConfig;
            //Log.Battle.D("Character is instantiate");
        }
    }
}