using System;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Core.Zombies;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleFlow : IStartable, IDisposable
    {
        private readonly BattleController _battleController;
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly BattleFactory _battleFactory;
        private readonly ZombiesFactory _zombiesFactory;

        public BattleFlow(LoadingService loadingService,
            BattleFactory battleFactory,
            ZombiesFactory zombiesFactory,
            SceneManager sceneManager,
            BattleController battleController)
        {
            _loadingService = loadingService;
            _battleFactory = battleFactory;
            _zombiesFactory = zombiesFactory;
            _sceneManager = sceneManager;
            _battleController = battleController;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(_battleFactory);
            await _loadingService.BeginLoading(_zombiesFactory);
            await _loadingService.BeginLoading(_battleController);

            _battleController.StartBattle();
            Log.Battle.D("BattleFlow.Start()");
        }
    }
}