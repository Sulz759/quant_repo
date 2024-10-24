using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Train;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public sealed class BattleScope : LifetimeScope
    {
        [SerializeField] private TouchscreenInput _touchscreen;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_touchscreen).AsSelf();

            builder.Register<TrainFactory>(Lifetime.Singleton);
            builder.Register<BattleController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BattleFlow>();
        }
    }
}