using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace _Project.Develop.Architecture.Runtime.Core.Zombies
{
    public class ZombieSpawnController: MonoBehaviour
    {
        private LevelDataStorage _data;
        public List<WaveData> Waves { get; private set; }
        
        private bool _isInitialized;

        [Inject]
        private void Inject(LevelDataStorage data)
        {
            _data = data;
        }

        public void Initialize(string levelNumber)
        {
            if (_isInitialized) return;

            foreach (var level in _data.Container.levels.Where(level => level.levelNumber == levelNumber))
            {
                Waves = level.waves;
            }
        }
    }
}