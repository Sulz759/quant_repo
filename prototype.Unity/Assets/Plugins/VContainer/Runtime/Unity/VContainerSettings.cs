using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VContainer.Unity
{
    public sealed class VContainerSettings : ScriptableObject
    {
        private static LifetimeScope rootLifetimeScopeInstance;

        [SerializeField] [Tooltip("Set the Prefab to be the parent of the entire Project.")]
        public LifetimeScope RootLifetimeScope;

        [SerializeField]
        [Tooltip(
            "Enables the collection of information that can be viewed in the VContainerDiagnosticsWindow. Note: Performance degradation")]
        public bool EnableDiagnostics;

        [SerializeField] [Tooltip("Disables script modification for LifetimeScope scripts.")]
        public bool DisableScriptModifier;

        [SerializeField]
        [Tooltip(
            "Removes (Clone) postfix in IObjectResolver.Instantiate() and IContainerBuilder.RegisterComponentInNewPrefab().")]
        public bool RemoveClonePostfix;

        public static VContainerSettings Instance { get; private set; }
        public static bool DiagnosticsEnabled => Instance != null && Instance.EnableDiagnostics;

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                Instance = this;

                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.isLoaded)
                {
                    OnFirstSceneLoaded(activeScene, default);
                }
                else
                {
                    SceneManager.sceneLoaded -= OnFirstSceneLoaded;
                    SceneManager.sceneLoaded += OnFirstSceneLoaded;
                }
            }
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public LifetimeScope GetOrCreateRootLifetimeScopeInstance()
        {
            if (RootLifetimeScope != null && rootLifetimeScopeInstance == null)
            {
                var activeBefore = RootLifetimeScope.gameObject.activeSelf;
                RootLifetimeScope.gameObject.SetActive(false);

                rootLifetimeScopeInstance = Instantiate(RootLifetimeScope);
                DontDestroyOnLoad(rootLifetimeScopeInstance);
                rootLifetimeScopeInstance.gameObject.SetActive(true);

                RootLifetimeScope.gameObject.SetActive(activeBefore);
            }

            return rootLifetimeScopeInstance;
        }

        public bool IsRootLifetimeScopeInstance(LifetimeScope lifetimeScope)
        {
            return RootLifetimeScope == lifetimeScope || rootLifetimeScopeInstance == lifetimeScope;
        }

        private void OnFirstSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (RootLifetimeScope != null &&
                RootLifetimeScope.autoRun &&
                (rootLifetimeScopeInstance == null || rootLifetimeScopeInstance.Container == null))
                GetOrCreateRootLifetimeScopeInstance();
            SceneManager.sceneLoaded -= OnFirstSceneLoaded;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/VContainer/VContainer Settings")]
        public static void CreateAsset()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save VContainerSettings",
                "VContainerSettings",
                "asset",
                string.Empty);

            if (string.IsNullOrEmpty(path))
                return;

            var newSettings = CreateInstance<VContainerSettings>();
            AssetDatabase.CreateAsset(newSettings, path);

            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is VContainerSettings);
            preloadedAssets.Add(newSettings);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        public static void LoadInstanceFromPreloadAssets()
        {
            var preloadAsset = PlayerSettings.GetPreloadedAssets().FirstOrDefault(x => x is VContainerSettings);
            if (preloadAsset is VContainerSettings instance)
            {
                instance.OnDisable();
                instance.OnEnable();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            // For editor, we need to load the Preload asset manually.
            LoadInstanceFromPreloadAssets();
        }
#endif
    }
}