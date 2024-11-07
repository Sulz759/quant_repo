using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class LevelDataStorage: ILoadUnit
    {
        public LevelDataContainer Container;
        
        public UniTask Load()
        {
            var asset = AssetService.R.Load<TextAsset>(RuntimeConstants.Configs.LevelConfigFileName);
            JsonConvert.PopulateObject(asset.text, this);
            return UniTask.CompletedTask;
        }

        public LevelData GetLevelData(string number)
        {
            // bad realization, need to repair it
            
            var level = new LevelData();
            foreach (var conf in Container.levels.Where(level => level.levelNumber == number))
            {
                level = conf;
            }
            
            return level;
        }
    }

    [Serializable]
    public sealed class LevelDataContainer
    {
        public List<LevelData> levels;
    }

    [Serializable]
    public class LevelData
    {
        public string levelNumber;
        public string biomeName;
        public List<WaveData> waves;
    }

    [Serializable]
    public class WaveData
    {
        public string direction;
        public int zombiesCount;
    }
}