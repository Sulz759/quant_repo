using _Project.Develop.Architecture.Runtime.Bootstrap.Units;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly CheckpointFactory _checkpointFactory;
        private readonly GlobalMapController _globalMapController;
        private readonly SceneManager _sceneManager;

        public MetaFlow(LoadingService loadingService,
            CheckpointFactory checkpointFactory,
            GlobalMapController globalMapController,
            SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _checkpointFactory = checkpointFactory;
            _globalMapController = globalMapController;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(_checkpointFactory);
            await _loadingService.BeginLoading(_globalMapController, new GlobalMapConfiguration(3));
            _globalMapController.StartMetaGame();
            Log.Meta.D("MetaFlow.Start()");
            //_sceneManager.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}