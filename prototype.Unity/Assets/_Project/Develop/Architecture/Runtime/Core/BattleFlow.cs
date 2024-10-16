﻿using System;
using _Project.Develop.Architecture.Runtime.Bootstrap.Units;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public class BattleFlow : IStartable, IDisposable
    {
        private readonly LoadingService _loadingService;
        private readonly BattleController _battleController;
        private readonly TrainFactory _trainFactory;
        private readonly SceneManager _sceneManager;

        public BattleFlow(LoadingService loadingService, 
            TrainFactory trainFactory,
            SceneManager sceneManager, 
            BattleController battleController)
        {
            _loadingService = loadingService;
            _trainFactory = trainFactory;
            _sceneManager = sceneManager;
            _battleController = battleController;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(_trainFactory);
            await _loadingService.BeginLoading(_battleController);
            
            _battleController.StartBattle();
            Log.Battle.D("BattleFlow.Start()");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}