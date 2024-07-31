using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Biome;
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
        private NodeView _nodePrefab;
        private BiomeView _biomePrefab;

        public NodeFactory(MetaConfiguration configs)
        {
            _configs = configs;
        }
        
        public UniTask Load()
        {
            _nodePrefab = Resources.Load<NodeView>(RuntimeConstants.Prefabs.Checkpoint);
            _biomePrefab = Resources.Load<BiomeView>("Prefabs/Meta/Bioms/" + $"{_configs.Container.biomeName}");
            return UniTask.CompletedTask;
        }

        public BiomeView CreateBiome()
        {
            var biom = Object.Instantiate(_biomePrefab);
            return biom;
        }

        public List<NodeView> CreateNodes()
        {
            var nodes = new List<NodeView>();
            ShuffleBag<int> intBag = new ShuffleBag<int> { 0, 1, 2, 3, 4 };
            
            foreach (var nodeConfig in _configs.Container.nodes)
            {
                var node = Object.Instantiate(_nodePrefab, 
                    new Vector3(nodeConfig.Position.X,nodeConfig.Position.Y,nodeConfig.Position.Z), 
                    Quaternion.identity);

                var nodeType = Node.Type[intBag.Generate()];

                node.Initialize(nodeConfig, nodeType);
                nodes.Add(node);
            }

            return nodes;
        }
    }
    
}