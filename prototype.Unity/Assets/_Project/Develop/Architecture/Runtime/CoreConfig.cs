using System;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime
{
    public sealed class CoreConfig : ILoadUnit
    {
        public CoreConfigContainer Container;

        public UniTask Load()
        {
            var asset = AssetService.R.Load<TextAsset>(RuntimeConstants.Configs.CoreConfigFileName);
            JsonConvert.PopulateObject(asset.text, this);
            return UniTask.CompletedTask;
        }

        public void SaveConfig()
        {
            var asset = AssetService.R.Load<TextAsset>(RuntimeConstants.Configs.CoreConfigFileName);

            asset = new TextAsset(JsonConvert.SerializeObject(Container));
        }
    }

    [Serializable]
    public class CoreConfigContainer
    {
        public string levelNumber;
        public TrainConfig TrainConfig;
    }

    public class TrainConfig
    {
    }
}