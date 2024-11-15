using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainView : MonoBehaviour
    {
        public RailwayView Railway { get; private set; }
        public List<WayView> Ways { get; private set; }
        
        public WayView CurrentWay { get; private set; }

        private TrainConfig _trainConfig;
        private TrainController _trainController;
        
        private TouchscreenInput _input;

        private Vector3 _velocity;
        public bool isInitialized { get; private set; }

        private void Start()
        {
            if (!isInitialized)
            {
                var input = FindObjectOfType<TouchscreenInput>();
                var railway = FindObjectOfType<RailwayView>();
                railway.Initialize();
                Initialize(new TrainConfig(), input, railway);
            }
        }

        public void Initialize(TrainConfig config, TouchscreenInput input, RailwayView railway)
        {
            _trainConfig = config;
            _input = input;
            
            Railway = railway;
            Ways = Railway.GetWays();
            CurrentWay = Ways[0];
            transform.position = CurrentWay.transform.position;
            transform.rotation = CurrentWay.transform.rotation;
            
            _trainController = new TrainController(this, input);
            
            isInitialized = true;
            Log.Battle.D("Train is instantiate");
        }

        public void NewCurrentWay(WayView way)
        {
            CurrentWay = way;
        }

        private void Update()
        {
            _trainController.Update();
        }
    }
}