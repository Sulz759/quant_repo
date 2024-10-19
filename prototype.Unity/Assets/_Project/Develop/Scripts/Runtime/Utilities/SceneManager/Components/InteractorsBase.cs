using System;
using System.Collections.Generic;

namespace _Project.Architecture
{
    public class InteractorsBase
    {
        private Dictionary<Type, Interactor> interactorsMap;
        private readonly SceneConfig sceneConfig;

        public InteractorsBase(SceneConfig sceneConfig)
        {
            this.sceneConfig = sceneConfig;
        }

        public void CreateAllInteractors()
        {
            interactorsMap = sceneConfig.CreateAllInteractors();
        }

        public void SendOnCreateToAllInteractors()
        {
            var allInteractors = interactorsMap.Values;
            foreach (var interactor in allInteractors) interactor.OnCreate();
        }

        public void InitializeAllInteractors()
        {
            var allInteractors = interactorsMap.Values;
            foreach (var interactor in allInteractors) interactor.Initialize();
        }

        public void SendOnStartToAllInteractors()
        {
            var allInteractors = interactorsMap.Values;
            foreach (var interactor in allInteractors) interactor.OnStart();
        }

        public T GetInteractor<T>() where T : Interactor
        {
            var type = typeof(T);
            return (T)interactorsMap[type];
        }
    }
}