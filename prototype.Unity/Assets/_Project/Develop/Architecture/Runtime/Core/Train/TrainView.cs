﻿using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainView : MonoBehaviour
    {
        private TrainConfig _trainConfig;
        private TrainController _trainController;

        public void Initialize(TrainConfig config, IInput input)
        {
            _trainConfig = config;
            _trainController = new TrainController(input);
            Log.Battle.D("Train is instantiate");
        }

        //public void Move()
    }
}