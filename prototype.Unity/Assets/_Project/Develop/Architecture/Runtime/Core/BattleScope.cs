using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Train;
using _Project.Develop.Architecture.Runtime.Core.Zombies;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public sealed class BattleScope : LifetimeScope
    {
        [SerializeField] private TouchscreenInput _touchscreen;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_touchscreen).AsSelf(); // Don't sure what it to need

            builder.Register<BattleFactory>(Lifetime.Singleton);
            builder.Register<ZombiesFactory>(Lifetime.Singleton);
            builder.Register<BattleController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BattleFlow>();
        }
    }
}