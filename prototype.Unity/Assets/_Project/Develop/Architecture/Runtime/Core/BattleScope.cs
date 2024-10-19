using _Project.Develop.Architecture.Runtime.Core.Input;
using _Project.Develop.Architecture.Runtime.Core.Train;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Core
{
    public sealed class BattleScope : LifetimeScope
    {
        [SerializeField] private TachscreenInput _tachscreen;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tachscreen).AsSelf();

            builder.Register<TrainFactory>(Lifetime.Singleton);
            builder.Register<BattleController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BattleFlow>();
        }
    }
}