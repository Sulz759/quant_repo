using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainController
    {
        private TrainView _view;
        private TouchscreenInput _input;
        
        public Queue<Vector3> Points { get; private set; }
        public Vector3 CurrentPoint { get; private set; }
        
        public TrainController(TrainView view,TouchscreenInput input)
        {
            _view = view;
            _input = input;
        }

        public void Initialize()
        {
            Points = new Queue<Vector3>();
            
            _input.OnTrainMoveEvent.AddListener(Move);
        }

        public void GetCurrentPoint()
        {
            CurrentPoint = Points.Dequeue();
        }
        
        private void CalculateWay(Vector3 finish)
        {
            Points.Clear();
            var start = _view.transform.position;
            var point = start + finish;
            Points.Enqueue(point);
            Points.Enqueue(finish);
        }

        private void Move(GameObject way)
        {
            CalculateWay(way.transform.position);
        }
    }
}