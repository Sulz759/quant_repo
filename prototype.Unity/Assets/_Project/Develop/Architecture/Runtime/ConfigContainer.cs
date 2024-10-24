using System;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime
{
    public sealed class ConfigContainer : ILoadUnit
    {
        public BattleConfigContainer Container;

        public UniTask Load()
        {
            var asset = AssetService.R.Load<TextAsset>(RuntimeConstants.Configs.CoreConfigFileName);
            JsonConvert.PopulateObject(asset.text, this);
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class BattleConfigContainer
    {
        public string biomeName;
        public TrainConfig TrainConfig;
    }

    public class TrainConfig
    {
    }
}