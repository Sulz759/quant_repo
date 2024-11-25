using System.Collections;
using _Project.Architecture.Scripts.Runtime.Utilities;
using _Project.Architecture.Scripts.Runtime.Utilities.SceneManager;
using UnityEngine.Events;

namespace _Project.Architecture.Scripts.Runtime
{
    public static class Game
    {
        public static UnityEvent OnGameInitializedEvent;

        public static SceneManagerBase sceneManager { get; private set; }

        public static void Run()
        {
            sceneManager = new SceneManagerItem();
            Coroutines.StartRoutine(InitializeGameRoutine());
        }

        public static void LoadScene(string sceneName)
        {
            sceneManager.LoadNewSceneAsync(sceneName);
        }

        private static IEnumerator InitializeGameRoutine()
        {
            sceneManager.InitScenesMap();
            yield return sceneManager.LoadCurrentSceneAsync();
            OnGameInitializedEvent?.Invoke();
        }

        public static T GetInteractor<T>() where T : Interactor
        {
            return sceneManager.GetInteractor<T>();
        }

        public static T GetRepository<T>() where T : Repository
        {
            return sceneManager.GetRepository<T>();
        }
    }
}