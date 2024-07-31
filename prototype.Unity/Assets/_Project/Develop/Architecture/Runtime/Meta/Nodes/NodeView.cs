
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeView: MonoBehaviour
    {
        public string type;
        private NodeConfig _nodeConfig;
        private NodeType _nodeType;
        [SerializeField] private Material material;
        
        
        public void Initialize(NodeConfig nodeConfig, NodeType nodeType)
        {
            _nodeConfig = nodeConfig;
            _nodeType = nodeType;
            GetNodeType();
        }

        private void GetNodeType()
        {
            _nodeType.Initialize();
            type = _nodeType.GetType().ToString();
            material.color = _nodeType.color;

            //Log.Meta.D($"{_nodeType.GetType().ToString()}");
        }
    }
}