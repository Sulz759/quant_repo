using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public sealed class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CheckpointFactory>(Lifetime.Singleton);
            builder.Register<GlobalMapController>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<MetaFlow>();
        }
    }
}