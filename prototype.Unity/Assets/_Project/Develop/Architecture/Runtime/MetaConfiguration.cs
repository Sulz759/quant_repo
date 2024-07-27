using System;
using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;

namespace _Project.Develop.Architecture.Runtime
{
    public class MetaConfiguration: ILoadUnit
    {
        public MetaConfigContainer Container;
        
        public UniTask Load()
        {
            var asset = AssetService.R.Load<TextAsset>(RuntimeConstants.Configs.ConfigFileName);
            JsonConvert.PopulateObject(asset.text, this);
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public sealed class MetaConfigContainer
    {
        public List<NodeConfig> nodes;

        /*public MetaConfigContainer (int nodesCount)
        {
            NodesCount = nodesCount;
            Initialize();
        }
        public IReadOnlyList<NodeConfig> NodesConfigs => _nodesConfigs;

        private readonly List<NodeConfig> _nodesConfigs = new ();

        public async void Initialize()
        {
            for (var i = 0; i < NodesCount; i++)
            {
                var node = new NodeConfig(new Vector3(i,0,i));
                _nodesConfigs.Add(node);
            }
        }*/
    }
    [Serializable]
    public class NodeConfig
    {
        public Vector3 Position;
    }
}