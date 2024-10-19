using System.Collections;
using UnityEngine;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
    public class Scene
    {
        private readonly InteractorsBase _interactorsBase;
        private readonly RepositoriesBase _repositoriesBase;

        public Scene(SceneConfig config)
        {
            sceneConfig = config;
            _interactorsBase = new InteractorsBase(config);
            _repositoriesBase = new RepositoriesBase(config);
        }

        public SceneConfig sceneConfig { get; }

        public Coroutine InitializeAsync()
        {
            return Coroutines.StartRoutine(InitializeRoutine());
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
            return _repositoriesBase.GetRepository<T>();
        }

        public T GetInteractor<T>() where T : Interactor
        {
            return _interactorsBase.GetInteractor<T>();
        }
    }
}