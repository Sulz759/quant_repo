using _Project.Develop.Architecture.Runtime.Utilities.StateMachine;

namespace _Project.Develop.Architecture.Runtime.Core.Train.StateMachine
{
    public class StateStandingTrain: FSMState
    {
        private TrainView _view;
        private TrainController _controller;
        public StateStandingTrain(FSM fsm, TrainController controller): base(fsm)
        {
            _controller = controller;
            _view = controller.View;
        }
        
    }
}