using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Biome;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class MetaController: ILoadUnit
    {
        public IReadOnlyList<NodeView> Checkpoints => _checkpoints;

        private List<NodeView> _checkpoints;
        private BiomeView _biome;
        private readonly NodeFactory _nodeFactory;

        public MetaController(NodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
        }
        public UniTask Load()
        {
            _biome = _nodeFactory.CreateBiome();
            _checkpoints = _nodeFactory.CreateNodes();
            
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
}