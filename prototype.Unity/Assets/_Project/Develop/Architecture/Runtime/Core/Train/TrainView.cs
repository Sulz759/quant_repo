using System;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainView : MonoBehaviour
    {
        private TrainConfig _trainConfig;
        private TrainController _trainController;

        private void Start()
        {
            // TESTS
            Initialize(new TrainConfig(), new TouchscreenInput());
        }

        public void Initialize(TrainConfig config, TouchscreenInput input)
        {
            _trainConfig = config;
            _trainController = new TrainController(input);
            _trainController.Initialize();
            Log.Battle.D("Train is instantiate");
        }

        //public void Move()
    }
}