using _Project.Develop.Architecture.Runtime.Bootstrap.Units;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly NodeFactory _nodeFactory;
        private readonly MetaController _metaController;
        private readonly SceneManager _sceneManager;

        public MetaFlow(LoadingService loadingService,
            NodeFactory nodeFactory,
            MetaController metaController,
            SceneManager sceneManager,
            MetaConfiguration configContainer)
        {
            _loadingService = loadingService;
            _nodeFactory = nodeFactory;
            _metaController = metaController;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(_nodeFactory);
            await _loadingService.BeginLoading(_metaController);
            _metaController.StartMetaGame();
            Log.Meta.D("MetaFlow.Start()");
            //_sceneManager.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}