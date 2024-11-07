using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainView : MonoBehaviour
    {
        private TrainConfig _trainConfig;
        private TrainController _trainController;
        private List<WayView> _ways;
        [SerializeField] private TouchscreenInput input;

        private Vector3 _velocity;
        public bool isInitialized { get; private set; }

        private void Start()
        {
            if (!isInitialized)
            {
                var ways = FindObjectOfType<RailwayView>();
                Initialize(new TrainConfig(), this.input, ways.GetWays());
                transform.position = _ways[0].transform.position;
            }
        }

        public void Initialize(TrainConfig config, TouchscreenInput input, List<WayView> ways)
        {
            isInitialized = true;

            _trainConfig = config;
            this.input = input;
            _ways = ways;
            _trainController = new TrainController(this,input);
            _trainController.Initialize();
            Log.Battle.D("Train is instantiate");
        }

        private void Update()
        {
            _trainController.Update();
        }
    }
}