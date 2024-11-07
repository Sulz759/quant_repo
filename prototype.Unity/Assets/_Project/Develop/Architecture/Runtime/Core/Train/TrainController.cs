using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Train.StateMachine;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using _Project.Develop.Architecture.Runtime.Utilities.StateMachine;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainController
    {
        public TrainView View { get; private set; }
        private TouchscreenInput _input;
        private FSM _fsm;
        
        public Queue<Vector3> Points { get; private set; }
        public Vector3 CurrentPoint { get; private set; }
        
        public TrainController(TrainView view,TouchscreenInput input)
        {
            View = view;
            _input = input;
        }

        public void Initialize()
        {
            Points = new Queue<Vector3>();
            Log.Battle.D(Points.Count.ToString());
            _fsm = new FSM();
            _fsm.AddState(new StateStandingTrain(_fsm, this));
            _fsm.AddState(new StateMovementTrain(_fsm, this));
            
            _fsm.SetState<StateStandingTrain>();
            
            _input.OnTrainMoveEvent.AddListener(Move);
        }

        public void GetCurrentPoint()
        {
            CurrentPoint = Points.Dequeue();
        }
        
        private void Move(GameObject way)
        {
            _fsm.SetState<StateMovementTrain>();
            CalculateWay(way.transform.position);
        }
        
        private void CalculateWay(Vector3 finish)
        {
            var start = View.transform.position;
            
            if (finish == start) return;
            
            Points.Clear();
            var midpoint = start + finish;
            Log.Battle.D(midpoint.ToString());
            Points.Enqueue(midpoint);
            Points.Enqueue(finish);

            if (midpoint.x + midpoint.y + midpoint.z >= 0)
            {
                
            }
            /*if (point.x < Mathf.Abs(0.01f) && point.z < Mathf.Abs(0.01f))
            {
                var x = point.x;
                var z = point.z;
                var point1 = new Vector3(z, start.y, x);
                Log.Battle.D(point1.ToString());
                Points.Enqueue(point1);
                var point2 = point1 + finish;
                Points.Enqueue(point2);
                Points.Enqueue(finish);
            }
            else
            {
                Points.Enqueue(point);
                Points.Enqueue(finish);
            }*/
        }

        public void Update()
        {
            _fsm.Update();
        }
    }
}