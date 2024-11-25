using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using _Project.Develop.Architecture.Runtime.Utilities.StateMachine;
using PlasticPipe.PlasticProtocol.Client;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainController
    {
        public TrainView View { get; private set; }
        private TouchscreenInput _input;
        private FSM _fsm;
        public readonly MovementParamsContainer MoveParams = new ();

        public TrainController(TrainView view,TouchscreenInput input)
        {
            View = view;
            _input = input;
            
            _fsm = new FSM();
            _fsm.AddState(new StateStandTrain(_fsm, this));
            _fsm.AddState(new StateMovementTrain(_fsm, this));
            _fsm.SetState<StateStandTrain>();

            _input.OnTrainMoveEvent.AddListener(Move);
        }

        public void Update()
        {
            _fsm.Update();
        }

        public void GetNextPoint()
        {
            var point = MoveParams.Points.Dequeue();
            MoveParams.NextPoint = point.Object.transform.position;
            foreach (var way in View.Railway.GetWays())
            {
                if (point.Index == way.index)
                {
                    View.NewCurrentWay(way);
                }
            }
        }
        
        private void Move(WayView targetWay)
        {
            foreach (var way in View.Ways)
            {
                if (way.index == targetWay.index)
                {
                    Log.Battle.D("Touch");
                    CreateTrainRoute(targetWay);
                }
            }
        }
        
        private void CreateTrainRoute(WayView way)
        {
            var start = View.CurrentWay;
            var finish = way;
            if (finish.index == start.index) return;
            
            MoveParams.Points.Clear();

            if (start.index < finish.index)
            {
                for (var i = start.index; i <= finish.index; i++)
                    MoveParams.Points.Enqueue(View.Railway.Checkpoints[i]);
            }
            else
            {
                for (var i = start.index; i < View.Railway.Checkpoints.Count; i++)
                    MoveParams.Points.Enqueue(View.Railway.Checkpoints[i]);
                for (var i = 0; i < finish.index; i++)
                    MoveParams.Points.Enqueue(View.Railway.Checkpoints[i]);
            }
            
            MoveParams.Points.Enqueue(new Checkpoint(finish.index, finish.gameObject));
                        
            _fsm.SetState<StateMovementTrain>();
        }

        
    }

    public class MovementParamsContainer
    {
        public WayView StartWay;
        public WayView FinishWay;
        public Queue<Checkpoint> Points;
        public Vector3 NextPoint;

        public MovementParamsContainer()
        {
            Points = new Queue<Checkpoint>();
        }
    }
}