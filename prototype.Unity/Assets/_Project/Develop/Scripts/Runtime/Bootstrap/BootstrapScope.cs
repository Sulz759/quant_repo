using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace _Project.Architecture.Scripts.Runtime.Bootstrap
{
    public class BootstrapScope: LifetimeScope
    {
        [SerializeField] private HelloScreen helloScreen;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.RegisterComponent(helloScreen);

        }
    }
}