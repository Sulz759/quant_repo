﻿using VContainer;
using VContainer.Unity;

namespace _Project.Develop.Architecture.Runtime.Loading
{
    public sealed class LoadingScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoadingFlow>();
        }
    }
}