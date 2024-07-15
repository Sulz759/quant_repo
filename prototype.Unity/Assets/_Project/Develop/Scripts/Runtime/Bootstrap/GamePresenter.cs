using VContainer;
using VContainer.Unity;

namespace _Project.Architecture.Scripts.Runtime.Bootstrap
{
    /*public class GamePresenter: ITickable
    {
        private readonly HelloWorldService _helloWorldService;

        public GamePresenter(HelloWorldService helloWorldService)
        {
            this._helloWorldService = helloWorldService;
        }

        public void Tick()
        {
            _helloWorldService.Hello();
        }
    }*/
    public class GamePresenter: IStartable
    {
        private readonly HelloWorldService _helloWorldService;
        private readonly HelloScreen _helloScreen;
        
        [Inject]
        public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
        {
            this._helloWorldService = helloWorldService;
            this._helloScreen = helloScreen;
        }

        public void Start()
        {
            _helloScreen.HelloButton.onClick.AddListener(() => _helloWorldService.Hello());
        }
    }
}