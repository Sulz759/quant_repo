using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public sealed class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<NodeFactory>(Lifetime.Singleton);
            builder.Register<MetaController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<MetaFlow>();
        }
    }
}