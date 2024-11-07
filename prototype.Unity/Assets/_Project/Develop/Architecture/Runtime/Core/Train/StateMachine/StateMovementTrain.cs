using _Project.Develop.Architecture.Runtime.Utilities.StateMachine;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Train.StateMachine
{
    public class StateMovementTrain : FSMState
    {
        private TrainView _view;
        private TrainController _controller;
        public StateMovementTrain(FSM fsm, TrainController controller): base(fsm)
        {
            _controller = controller;
            _view = controller.View;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            if (_view.transform.position == _controller.CurrentPoint)
            {
                if (_controller.Points.Count != 0)
                {
                    _controller.GetCurrentPoint();
                }
                else
                {
                    Fsm.SetState<StateStandingTrain>();
                }
            }
            else
            {
                Move();
            }
        }
        
        private void Move()
        {
            _view.transform.position = Vector3.Lerp(
                _view.transform.position,
                _controller.CurrentPoint,
                0.01f + Time.deltaTime
            );
        }
    }
}