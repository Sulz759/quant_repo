using System;
using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace _Project.Develop.Architecture.Runtime
{
    public class MetaConfiguration : ILoadUnit
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
        public string biomeName;
        public List<NodeConfig> nodes;
    }

    [Serializable]
    public class NodeConfig
    {
        public NodeType NodeType;
        public Vector3 pos;
    }
}