using _Project.Develop.Architecture.Runtime.Core.Input;

namespace _Project.Develop.Architecture.Runtime.Core.Train
{
    public class TrainController
    {
        private IInput _inputConfig;

        public TrainController(IInput config)
        {
            _inputConfig = config;
        }
    }
}