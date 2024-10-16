using _Project.Develop.Architecture.Runtime.Core.Train;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public sealed class BattleScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TrainFactory>(Lifetime.Singleton);
            builder.Register<BattleController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BattleFlow>();
        }
    }
}