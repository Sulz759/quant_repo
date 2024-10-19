using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VContainer.Unity
{
    internal sealed class PrefabComponentProvider : IInstanceProvider
    {
        private readonly IReadOnlyList<IInjectParameter> customParameters;
        private readonly IInjector injector;
        private readonly Func<IObjectResolver, Component> prefabFinder;
        private ComponentDestination destination;

        public PrefabComponentProvider(
            Func<IObjectResolver, Component> prefabFinder,
            IInjector injector,
            IReadOnlyList<IInjectParameter> customParameters,
            in ComponentDestination destination)
        {
            this.injector = injector;
            this.customParameters = customParameters;
            this.prefabFinder = prefabFinder;
            this.destination = destination;
        }

        public object SpawnInstance(IObjectResolver resolver)
        {
            var prefab = prefabFinder(resolver);
            var parent = destination.GetParent(resolver);

            var wasActive = prefab.gameObject.activeSelf;
            if (wasActive) prefab.gameObject.SetActive(false);

            var component = parent != null
                ? Object.Instantiate(prefab, parent)
                : Object.Instantiate(prefab);

            if (VContainerSettings.Instance != null && VContainerSettings.Instance.RemoveClonePostfix)
                component.name = prefab.name;

            try
            {
                injector.Inject(component, resolver, customParameters);
                destination.ApplyDontDestroyOnLoadIfNeeded(component);
            }
            finally
            {
                if (wasActive)
                {
                    prefab.gameObject.SetActive(true);
                    component.gameObject.SetActive(true);
                }
            }

            return component;
        }
    }
}