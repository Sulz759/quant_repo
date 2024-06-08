using System.Collections;
using UnityEngine.Events;

namespace _Project.Architecture
{
	public static class Game
	{
		public static UnityEvent OnGameInitializedEvent;
		
		public static bool isRunning;
		public static bool isPaused;
		
		public static SceneManagerBase sceneManager { get; private set; }
		
		public static void Run() 
		{
			sceneManager = new SceneManagerItem();
			Coroutines.StartRoutine(InitializeGameRoutine());
			isRunning = true;
		}
		public static void LoadNewScene(string sceneName) 
		{
			sceneManager.LoadNewSceneAsync(sceneName);
		}

		public static void ReturnToPastScene()
		{
			sceneManager.LoadNewSceneAsync(sceneManager.pastScene.sceneConfig.sceneName);
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