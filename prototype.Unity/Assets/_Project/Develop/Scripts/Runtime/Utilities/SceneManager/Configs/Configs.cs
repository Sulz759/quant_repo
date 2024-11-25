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

    public class MetaConfig : SceneConfig
    {
        public const string SCENE_NAME = "Meta";

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

    public class CoreConfig : SceneConfig
    {
        public const string SCENE_NAME = "Core";

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