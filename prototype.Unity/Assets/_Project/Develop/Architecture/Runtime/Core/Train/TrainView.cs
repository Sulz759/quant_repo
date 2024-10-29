using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainView : MonoBehaviour
    {
        private TrainConfig _trainConfig;
        private TrainController _trainController;
        [SerializeField] private TouchscreenInput input;

        private Vector3 _velocity;

        public bool isInitialized { get; private set; }

        private void Start()
        {
            if (!isInitialized)
            {
                Initialize(new TrainConfig(), this.input);
            }
        }

        public void Initialize(TrainConfig config, TouchscreenInput input)
        {
            isInitialized = true;
            
            _trainConfig = config;
            this.input = input;
            _trainController = new TrainController(this,input);
            _trainController.Initialize();
            Log.Battle.D("Train is instantiate");
        }
        private void Move()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                _trainController.CurrentPoint,
                0.1f + Time.deltaTime
            );
        }

        private void Update()
        {
            if (transform.position == _trainController.CurrentPoint)
            {
                if (_trainController.Points.Count != 0)
                {
                    _trainController.GetCurrentPoint();
                }
            }
            else
            {
                Move();
            }
        }
    }
}