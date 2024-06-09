using System;
using System.Collections.Generic;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager.Configs
{
	public class BootstrapConfig : SceneConfig
	{

		public const string SCENE_NAME = "Bootstrap";

		public override string sceneName => SCENE_NAME;



		public override Dictionary<Type, Repository> CreateAllRepositories()
		{
			var repositoriesMap = new Dictionary<Type, Repository>();
			
			//this.CreateRepository<BankRepository>(repositoriesMap);

			return repositoriesMap;
		}

		public override Dictionary<Type, Interactor> CreateAllInteractors()
		{
			var interactorsMap = new Dictionary<Type, Interactor>();
			
			//this.CreateInteractor<BankInteractor>(interactorsMap);

			return interactorsMap;
		}
	}
	public class GlobalMapConfig : SceneConfig
	{

		public const string SCENE_NAME = "GlobalMap";

		public override string sceneName => SCENE_NAME;



		public override Dictionary<Type, Repository> CreateAllRepositories()
		{
			var repositoriesMap = new Dictionary<Type, Repository>();
			
			//this.CreateRepository<BankRepository>(repositoriesMap);

			return repositoriesMap;
		}

		public override Dictionary<Type, Interactor> CreateAllInteractors()
		{
			var interactorsMap = new Dictionary<Type, Interactor>();
			
			//this.CreateInteractor<BankInteractor>(interactorsMap);

			return interactorsMap;
		}
	}
	public class FightSceneConfig : SceneConfig
	{

		public const string SCENE_NAME = "FightScene";

		public override string sceneName => SCENE_NAME;



		public override Dictionary<Type, Repository> CreateAllRepositories()
		{
			var repositoriesMap = new Dictionary<Type, Repository>();
			
			//this.CreateRepository<BankRepository>(repositoriesMap);

			return repositoriesMap;
		}

		public override Dictionary<Type, Interactor> CreateAllInteractors()
		{
			var interactorsMap = new Dictionary<Type, Interactor>();
			
			//this.CreateInteractor<BankInteractor>(interactorsMap);

			return interactorsMap;
		}
	}
}