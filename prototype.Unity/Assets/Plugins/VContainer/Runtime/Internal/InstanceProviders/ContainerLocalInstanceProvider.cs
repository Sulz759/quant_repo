using System;

namespace VContainer.Internal
{
    internal sealed class ContainerLocalInstanceProvider : IInstanceProvider
    {
        private readonly Registration valueRegistration;
        private readonly Type wrappedType;

        public ContainerLocalInstanceProvider(Type wrappedType, Registration valueRegistration)
        {
            this.wrappedType = wrappedType;
            this.valueRegistration = valueRegistration;
        }

        public object SpawnInstance(IObjectResolver resolver)
        {
            object value;

            if (resolver is ScopedContainer scope &&
                valueRegistration.Provider is CollectionInstanceProvider collectionProvider)
                using (ListPool<Registration>.Get(out var entirelyRegistrations))
                {
                    collectionProvider.CollectFromParentScopes(scope, entirelyRegistrations, true);
                    value = collectionProvider.SpawnInstance(resolver, entirelyRegistrations);
                }
            else
                value = resolver.Resolve(valueRegistration);

            var parameterValues = CappedArrayPool<object>.Shared8Limit.Rent(1);
            try
            {
                parameterValues[0] = value;
                return Activator.CreateInstance(wrappedType, parameterValues);
            }
            finally
            {
                CappedArrayPool<object>.Shared8Limit.Return(parameterValues);
            }
        }
    }
}