using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer.Unity
{
    [DefaultExecutionOrder(-5000)]
    public partial class LifetimeScope : MonoBehaviour, IDisposable
    {
        private static readonly Stack<LifetimeScope> GlobalOverrideParents = new();
        private static readonly Stack<IInstaller> GlobalExtraInstallers = new();
        private static readonly object SyncRoot = new();

        [SerializeField] public ParentReference parentReference;

        [SerializeField] public bool autoRun = true;

        [SerializeField] protected List<GameObject> autoInjectGameObjects;

        private readonly List<IInstaller> localExtraInstallers = new();

        private string scopeName;

        public IObjectResolver Container { get; private set; }
        public LifetimeScope Parent { get; private set; }

        public bool IsRoot => VContainerSettings.Instance != null &&
                              VContainerSettings.Instance.IsRootLifetimeScopeInstance(this);

        protected virtual void Awake()
        {
            if (VContainerSettings.DiagnosticsEnabled && string.IsNullOrEmpty(scopeName))
                scopeName = $"{name} ({gameObject.GetInstanceID()})";
            try
            {
                Parent = GetRuntimeParent();
                if (autoRun) Build();
            }
            catch (VContainerParentTypeReferenceNotFound) when (!IsRoot)
            {
                if (WaitingList.Contains(this)) throw;
                EnqueueAwake(this);
            }
        }

        protected virtual void OnDestroy()
        {
            DisposeCore();
        }

        public void Dispose()
        {
            DisposeCore();
            if (this != null) Destroy(gameObject);
        }

        private static LifetimeScope Create(IInstaller installer = null)
        {
            var gameObject = new GameObject("LifetimeScope");
            gameObject.SetActive(false);
            var newScope = gameObject.AddComponent<LifetimeScope>();
            if (installer != null) newScope.localExtraInstallers.Add(installer);
            gameObject.SetActive(true);
            return newScope;
        }

        public static LifetimeScope Create(Action<IContainerBuilder> configuration)
        {
            return Create(new ActionInstaller(configuration));
        }

        public static ParentOverrideScope EnqueueParent(LifetimeScope parent)
        {
            return new ParentOverrideScope(parent);
        }

        public static ExtraInstallationScope Enqueue(Action<IContainerBuilder> installing)
        {
            return new ExtraInstallationScope(new ActionInstaller(installing));
        }

        public static ExtraInstallationScope Enqueue(IInstaller installer)
        {
            return new ExtraInstallationScope(installer);
        }

        [Obsolete("LifetimeScope.PushParent is obsolete. Use LifetimeScope.EnqueueParent instead.", false)]
        public static ParentOverrideScope PushParent(LifetimeScope parent)
        {
            return new ParentOverrideScope(parent);
        }

        [Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
        public static ExtraInstallationScope Push(Action<IContainerBuilder> installing)
        {
            return Enqueue(installing);
        }

        [Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
        public static ExtraInstallationScope Push(IInstaller installer)
        {
            return Enqueue(installer);
        }

        public static LifetimeScope Find<T>(Scene scene) where T : LifetimeScope
        {
            return Find(typeof(T), scene);
        }

        public static LifetimeScope Find<T>() where T : LifetimeScope
        {
            return Find(typeof(T));
        }

        private static LifetimeScope Find(Type type, Scene scene)
        {
            using (ListPool<GameObject>.Get(out var buffer))
            {
                scene.GetRootGameObjects(buffer);
                foreach (var gameObject in buffer)
                {
                    var found = gameObject.GetComponentInChildren(type) as LifetimeScope;
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        private static LifetimeScope Find(Type type)
        {
#if UNITY_2022_1_OR_NEWER
            return (LifetimeScope)FindAnyObjectByType(type);
#else
            return (LifetimeScope)FindObjectOfType(type);
#endif
        }

        protected virtual void Configure(IContainerBuilder builder)
        {
        }

        public void DisposeCore()
        {
            Container?.Dispose();
            Container = null;
            CancelAwake(this);
            if (VContainerSettings.DiagnosticsEnabled) DiagnositcsContext.RemoveCollector(scopeName);
        }

        public void Build()
        {
            if (Parent == null)
                Parent = GetRuntimeParent();

            if (Parent != null)
            {
                if (VContainerSettings.Instance != null && Parent.IsRoot)
                    if (Parent.Container == null)
                        Parent.Build();

                // ReSharper disable once PossibleNullReferenceException
                Parent.Container.CreateScope(builder =>
                {
                    builder.RegisterBuildCallback(SetContainer);
                    builder.ApplicationOrigin = this;
                    builder.Diagnostics = VContainerSettings.DiagnosticsEnabled
                        ? DiagnositcsContext.GetCollector(scopeName)
                        : null;
                    InstallTo(builder);
                });
            }
            else
            {
                var builder = new ContainerBuilder
                {
                    ApplicationOrigin = this,
                    Diagnostics = VContainerSettings.DiagnosticsEnabled
                        ? DiagnositcsContext.GetCollector(scopeName)
                        : null
                };
                builder.RegisterBuildCallback(SetContainer);
                InstallTo(builder);
                builder.Build();
            }

            AwakeWaitingChildren(this);
        }

        private void SetContainer(IObjectResolver container)
        {
            Container = container;
            AutoInjectAll();
        }

        public TScope CreateChild<TScope>(IInstaller installer = null)
            where TScope : LifetimeScope
        {
            var childGameObject = new GameObject("LifetimeScope (Child)");
            childGameObject.SetActive(false);
            if (IsRoot)
                DontDestroyOnLoad(childGameObject);
            else
                childGameObject.transform.SetParent(transform, false);
            var child = childGameObject.AddComponent<TScope>();
            if (installer != null) child.localExtraInstallers.Add(installer);
            child.parentReference.Object = this;
            childGameObject.SetActive(true);
            return child;
        }

        public LifetimeScope CreateChild(IInstaller installer = null)
        {
            return CreateChild<LifetimeScope>(installer);
        }

        public TScope CreateChild<TScope>(Action<IContainerBuilder> installation)
            where TScope : LifetimeScope
        {
            return CreateChild<TScope>(new ActionInstaller(installation));
        }

        public LifetimeScope CreateChild(Action<IContainerBuilder> installation)
        {
            return CreateChild<LifetimeScope>(new ActionInstaller(installation));
        }

        public TScope CreateChildFromPrefab<TScope>(TScope prefab, IInstaller installer = null)
            where TScope : LifetimeScope
        {
            var wasActive = prefab.gameObject.activeSelf;
            if (wasActive) prefab.gameObject.SetActive(false);
            var child = Instantiate(prefab, transform, false);
            if (installer != null) child.localExtraInstallers.Add(installer);
            child.parentReference.Object = this;
            if (wasActive)
            {
                prefab.gameObject.SetActive(true);
                child.gameObject.SetActive(true);
            }

            return child;
        }

        public TScope CreateChildFromPrefab<TScope>(TScope prefab, Action<IContainerBuilder> installation)
            where TScope : LifetimeScope
        {
            return CreateChildFromPrefab(prefab, new ActionInstaller(installation));
        }

        private void InstallTo(IContainerBuilder builder)
        {
            Configure(builder);

            foreach (var installer in localExtraInstallers) installer.Install(builder);
            localExtraInstallers.Clear();

            lock (SyncRoot)
            {
                foreach (var installer in GlobalExtraInstallers) installer.Install(builder);
            }

            builder.RegisterInstance(this).AsSelf();
            EntryPointsBuilder.EnsureDispatcherRegistered(builder);
        }

        private LifetimeScope GetRuntimeParent()
        {
            if (IsRoot) return null;

            if (parentReference.Object != null)
                return parentReference.Object;

            // Find in scene via type
            if (parentReference.Type != null && parentReference.Type != GetType())
            {
                var found = Find(parentReference.Type);
                if (found != null && found.Container != null) return found;
                throw new VContainerParentTypeReferenceNotFound(
                    parentReference.Type,
                    $"{name} could not found parent reference of type : {parentReference.Type}");
            }

            lock (SyncRoot)
            {
                if (GlobalOverrideParents.Count > 0) return GlobalOverrideParents.Peek();
            }

            // Find root from settings
            if (VContainerSettings.Instance != null)
                return VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();

            return null;
        }

        private void AutoInjectAll()
        {
            if (autoInjectGameObjects == null)
                return;

            foreach (var target in autoInjectGameObjects)
                if (target != null) // Check missing reference
                    Container.InjectGameObject(target);
        }

        public readonly struct ParentOverrideScope : IDisposable
        {
            public ParentOverrideScope(LifetimeScope nextParent)
            {
                lock (SyncRoot)
                {
                    GlobalOverrideParents.Push(nextParent);
                }
            }

            public void Dispose()
            {
                lock (SyncRoot)
                {
                    GlobalOverrideParents.Pop();
                }
            }
        }

        public readonly struct ExtraInstallationScope : IDisposable
        {
            public ExtraInstallationScope(IInstaller installer)
            {
                lock (SyncRoot)
                {
                    GlobalExtraInstallers.Push(installer);
                }
            }

            void IDisposable.Dispose()
            {
                lock (SyncRoot)
                {
                    GlobalExtraInstallers.Pop();
                }
            }
        }
    }
}