using _Project.Develop.Architecture.Runtime.Utilities;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.Register<SceneManager>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}