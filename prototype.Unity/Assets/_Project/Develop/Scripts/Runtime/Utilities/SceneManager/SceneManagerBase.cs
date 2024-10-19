using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
    public abstract class SceneManagerBase
    {
        public UnityEvent OnSceneLoadedEvent = new(); // ранее возвращал Scene, убрал за ненадобностью

        protected Dictionary<string, SceneConfig> sceneConfigMap;

        public SceneManagerBase()
        {
            sceneConfigMap = new Dictionary<string, SceneConfig>();
        }

        public Scene scene { get; private set; }
        public bool isLoading { get; private set; }

        public abstract void InitScenesMap();

        public Coroutine LoadCurrentSceneAsync()
        {
            if (isLoading) throw new Exception("Scene is loading now");
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var config = sceneConfigMap[sceneName];
            return Coroutines.StartRoutine(LoadCurrentSceneRoutine(config));
        }

        private IEnumerator LoadCurrentSceneRoutine(SceneConfig sceneConfig)
        {
            isLoading = true;

            yield return Coroutines.StartRoutine(InitializeSceneRoutine(sceneConfig));

            isLoading = false;
            OnSceneLoadedEvent?.Invoke();
        }

        public Coroutine LoadNewSceneAsync(string sceneName)
        {
            if (isLoading) throw new Exception("Scene is loading now");
            var config = sceneConfigMap[sceneName];
            return Coroutines.StartRoutine(LoadNewSceneRoutine(config));
        }

        private IEnumerator LoadNewSceneRoutine(SceneConfig sceneConfig)
        {
            isLoading = true;

            yield return Coroutines.StartRoutine(LoadSceneRoutine(sceneConfig));
            yield return Coroutines.StartRoutine(InitializeSceneRoutine(sceneConfig));

            isLoading = false;
            OnSceneLoadedEvent?.Invoke();
        }

        private IEnumerator LoadSceneRoutine(SceneConfig sceneConfig)
        {
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneConfig.sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9f) yield return null;
            async.allowSceneActivation = true;
        }

        private IEnumerator InitializeSceneRoutine(SceneConfig sceneConfig)
        {
            scene = new Scene(sceneConfig);
            yield return scene.InitializeAsync();
        }

        public T GetRepository<T>() where T : Repository
        {
            return scene.GetRepository<T>();
        }

        public T GetInteractor<T>() where T : Interactor
        {
            return scene.GetInteractor<T>();
        }
    }
}