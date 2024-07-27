using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeFactory: ILoadUnit
    {
        private readonly MetaConfiguration _configs;
        private NodeView _prefab;

        public NodeFactory(MetaConfiguration configs)
        {
            _configs = configs;
        }
        
        public UniTask Load()
        {
            _prefab = Resources.Load<NodeView>("Prefabs/Meta/Checkpoint");
            return UniTask.CompletedTask;
        }

        public List<NodeView> CreateCheckpoints()
        {
            var nodes = new List<NodeView>();
            
            foreach (var nodeConfig in _configs.Container.nodes)
            {
                // какой-то колхоз, надо переделать передачу координат
                var node = Object.Instantiate(_prefab, 
                    new Vector3(nodeConfig.Position.X,nodeConfig.Position.Y,nodeConfig.Position.Z), 
                    Quaternion.identity);

                node.Initialize(nodeConfig);
                nodes.Add(node);
            }

            return nodes;
        }
    }
}