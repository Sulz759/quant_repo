using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainController
    {
        private TouchscreenInput _input;
        

        public TrainController(TouchscreenInput input)
        {
            _input = input;
        }

        public void Initialize()
        {
            _input.OnTrainMoveEvent.AddListener(Move);
        }

        private static void Move()
        {
            Log.Battle.D("It's trail");
        }
    }
}