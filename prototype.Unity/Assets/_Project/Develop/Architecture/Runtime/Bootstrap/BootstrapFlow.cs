using _Project.Develop.Architecture.Runtime.Bootstrap.Units;
using _Project.Develop.Architecture.Runtime.Core;
using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly MetaConfiguration _metaConfiguration;
        private readonly CoreConfig _coreConfig;
        private readonly LevelDataStorage _levelDataStorage;
        public BootstrapFlow(LoadingService loadingService,
            SceneManager sceneManager,
            CoreConfig coreConfig,
            MetaConfiguration metaConfiguration,
            LevelDataStorage levelDataStorage)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _coreConfig = coreConfig;
            _metaConfiguration = metaConfiguration;
            _levelDataStorage = levelDataStorage;
        }

        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_coreConfig);
            await _loadingService.BeginLoading(_metaConfiguration);
            await _loadingService.BeginLoading(_levelDataStorage);

            _sceneManager.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}