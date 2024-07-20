using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}