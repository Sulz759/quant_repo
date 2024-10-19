using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Biome;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeFactory : ILoadUnit
    {
        private readonly MetaConfiguration _configs;
        private BiomeView _biomePrefab;
        private NodeView _nodePrefab;

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
            return Object.Instantiate(_biomePrefab);
            ;
        }

        public List<NodeView> CreateNodes()
        {
            var nodes = new List<NodeView>();

            var generator = new ShuffleBag<NodeType>();

            generator.Add(Node.Types[0], 1, 1);
            generator.Add(Node.Types[1], 1, 3);
            generator.Add(Node.Types[2], 1, 5);
            generator.Add(Node.Types[3], 1, 5);
            generator.Add(Node.Types[4], 1, 5);

            foreach (var config in _configs.Container.nodes)
            {
                var position = new Vector3(config.pos.X, config.pos.Y, config.pos.Z);
                var node = Object.Instantiate(_nodePrefab, new Vector3(config.pos.X, config.pos.Y, config.pos.Z),
                    Quaternion.identity);

                var nodeType = generator.GetNext();

                node.Initialize(config, nodeType);
                nodes.Add(node);
            }

            return nodes;
        }
    }
}