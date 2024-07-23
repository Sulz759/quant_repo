using _Project.Develop.Architecture.Runtime.Bootstrap.Units;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly ConfigContainer _configContainer;

        public BootstrapFlow(LoadingService loadingService, 
            SceneManager sceneManager, 
            ConfigContainer configContainer)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _configContainer = configContainer;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_configContainer);

            _sceneManager.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
