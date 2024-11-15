using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using _Project.Develop.Architecture.Runtime.Utilities.StateMachine;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class StateStandTrain: FSMState
    {
        private TrainController _controller;
        private TrainView _view;
        public StateStandTrain(FSM fsm, TrainController controller): base(fsm)
        {
            _controller = controller;
            _view = controller.View;
        }

        public override void Enter()
        {
            base.Enter();
            Log.Battle.D("Stand");
        }
    }
    
    public class StateMovementTrain : FSMState
    {
        private TrainController _controller;
        private TrainView _view;
        public StateMovementTrain(FSM fsm, TrainController controller): base(fsm)
        {
            _controller = controller;
            _view = controller.View;
        }
        public override void Enter()
        {
            base.Enter();
            Log.Battle.D("Move");
            _controller.GetNextPoint();
        }

        public override void Update()
        {
            base.Update();
            if (_view.transform.position == _controller.MoveParams.NextPoint)
            {
                if (_controller.MoveParams.Points.Count != 0)
                {
                    _controller.GetNextPoint();
                }
                else
                {
                    Fsm.SetState<StateStandTrain>();
                }
            }
            else
            {
                Move();
            }
        }
        
        private void Move()
        {
            var transform = _view.transform;
            
            transform.position = Vector3.MoveTowards(
                transform.position,
                _controller.MoveParams.NextPoint,
                0.0025f + Time.deltaTime
            );
            transform.rotation = _view.CurrentWay.transform.rotation;
        }
    }
}