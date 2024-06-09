﻿using System.Collections;
using UnityEngine;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
	public class Scene
	{
		private InteractorsBase _interactorsBase;
		private RepositoriesBase _repositoriesBase;
		public SceneConfig sceneConfig { get; private set; }

		public Scene(SceneConfig config)
		{
			this.sceneConfig = config;
			this._interactorsBase = new InteractorsBase(config);
			this._repositoriesBase = new RepositoriesBase(config);
		}

		public Coroutine InitializeAsync() 
		{
			return Coroutines.StartRoutine(this.InitializeRoutine());
		}

		private IEnumerator InitializeRoutine()
		{
			_interactorsBase.CreateAllInteractors();
			_repositoriesBase.CreateAllRepositories();
			yield return null;

			_interactorsBase.SendOnCreateToAllInteractors();
			_repositoriesBase.SendOnCreateToAllRepositories();
			yield return null;

			_interactorsBase.InitializeAllInteractors();
			_repositoriesBase.InitializeAllRepositories();
			yield return null;

			_interactorsBase.SendOnStartToAllInteractors();
			_repositoriesBase.SendOnStartToAllRepositories();
			yield return null;
			Debug.Log($"{sceneConfig.sceneName} is loading");
		}

		public T GetRepository<T>() where T : Repository 
		{
			return this._repositoriesBase.GetRepository<T>();
		}

		public T GetInteractor<T>() where T : Interactor
		{
			return this._interactorsBase.GetInteractor<T>();
		}

	}
}