using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeView: MonoBehaviour
    {
        public string type;
        private NodeConfig _nodeConfig;
        private INodeType _nodeType;
        
        
        public void Initialize(NodeConfig nodeConfig, INodeType nodeType)
        {
            _nodeConfig = nodeConfig;
            _nodeType = nodeType;
            GetNodeType();
        }

        public void GetNodeType()
        {
            type = _nodeType.GetType().ToString();
            //Log.Meta.D($"{_nodeType.GetType().ToString()}");
        }
    }
}