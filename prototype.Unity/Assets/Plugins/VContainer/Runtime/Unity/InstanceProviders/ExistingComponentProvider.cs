using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VContainer.Unity
{
    internal sealed class ExistingComponentProvider : IInstanceProvider
    {
        private readonly IReadOnlyList<IInjectParameter> customParameters;
        private readonly bool dontDestroyOnLoad;
        private readonly IInjector injector;
        private readonly object instance;

        public ExistingComponentProvider(
            object instance,
            IInjector injector,
            IReadOnlyList<IInjectParameter> customParameters,
            bool dontDestroyOnLoad = false)
        {
            this.instance = instance;
            this.customParameters = customParameters;
            this.injector = injector;
            this.dontDestroyOnLoad = dontDestroyOnLoad;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver)
        {
            injector.Inject(instance, resolver, customParameters);
            if (dontDestroyOnLoad)
            {
                if (instance is Object component)
                    Object.DontDestroyOnLoad(component);
                else
                    throw new VContainerException(instance.GetType(),
                        $"Cannot apply `DontDestroyOnLoad`. {instance.GetType().Name} is not a UnityEngine.Object");
            }

            return instance;
        }
    }
}