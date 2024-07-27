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
        private readonly MetaConfiguration _metaConfiguration;


        public BootstrapFlow(LoadingService loadingService, 
            SceneManager sceneManager, 
            ConfigContainer configContainer,
            MetaConfiguration metaConfiguration)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _configContainer = configContainer;
            _metaConfiguration = metaConfiguration;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_configContainer);
            await _loadingService.BeginLoading(_metaConfiguration);

            _sceneManager.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
