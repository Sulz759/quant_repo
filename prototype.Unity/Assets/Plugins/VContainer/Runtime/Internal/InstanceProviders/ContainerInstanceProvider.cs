using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
    internal sealed class ContainerInstanceProvider : IInstanceProvider
    {
        public static readonly ContainerInstanceProvider Default = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver)
        {
            return resolver;
        }
    }
}