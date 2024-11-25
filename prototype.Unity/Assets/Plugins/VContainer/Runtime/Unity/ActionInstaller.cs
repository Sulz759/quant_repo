using System;

namespace VContainer.Unity
{
    public sealed class ActionInstaller : IInstaller
    {
        private readonly Action<IContainerBuilder> configuration;

        public ActionInstaller(Action<IContainerBuilder> configuration)
        {
            this.configuration = configuration;
        }

        public void Install(IContainerBuilder builder)
        {
            configuration(builder);
        }

        public static implicit operator ActionInstaller(Action<IContainerBuilder> installation)
        {
            return new ActionInstaller(installation);
        }
    }
}