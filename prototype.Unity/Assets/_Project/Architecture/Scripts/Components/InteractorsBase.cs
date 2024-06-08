using System;
using System.Collections.Generic;

namespace _Project.Architecture
{
	public class InteractorsBase
	{
		private Dictionary<Type, Interactor> interactorsMap;
		private SceneConfig sceneConfig;

		public InteractorsBase(SceneConfig sceneConfig)
		{
			this.sceneConfig = sceneConfig;
		}

		public void CreateAllInteractors()
		{
			this.interactorsMap = this.sceneConfig.CreateAllInteractors();
		}

		public void SendOnCreateToAllInteractors() 
		{
			var allInteractors = this.interactorsMap.Values;
			foreach (var interactor in allInteractors) 
			{
				interactor.OnCreate();
			}
		}
		public void InitializeAllInteractors()
		{
			var allInteractors = this.interactorsMap.Values;
			foreach (var interactor in allInteractors)
			{
				interactor.Initialize();
			}
		}
		public void SendOnStartToAllInteractors()
		{
			var allInteractors = this.interactorsMap.Values;
			foreach (var interactor in allInteractors)
			{
				interactor.OnStart();
			}
		}

		public T GetInteractor<T>() where T : Interactor 
		{
			var type = typeof(T);
			return (T) this.interactorsMap[type];
		}
	}
}