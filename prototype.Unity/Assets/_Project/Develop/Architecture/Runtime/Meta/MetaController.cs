using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Biome;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using _Project.Develop.Architecture.Runtime.Meta.Tests;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class MetaController : ILoadUnit
    {
        private readonly NodeFactory _nodeFactory;
        private BiomeView _biome;

        public MetaController(NodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
        }

        public IReadOnlyList<NodeView> Checkpoints => _checkpoints;

        public List<NodeView> _checkpoints { get; private set; }

        public UniTask Load()
        {
            _biome = _nodeFactory.CreateBiome();
            _checkpoints = _nodeFactory.CreateNodes();

            CreateNodesTest(); // for tests

            Log.Meta.D("Meta is loading");
            return UniTask.CompletedTask;
        }

        public void StartMetaGame()
        {
            foreach (var checkpoint in _checkpoints) checkpoint.gameObject.SetActive(true);

            Log.Meta.D("Start Meta Game");
        }

        //  *** Tests ***
        private void CreateNodesTest()
        {
            var test = new GameObject("Test").AddComponent<ShuffleBagTest>();
            test.factory = _nodeFactory;
            test.nodes = _checkpoints;
        }
    }
}