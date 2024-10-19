using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
    internal static class RuntimeTypeCache
    {
        private static readonly ConcurrentDictionary<Type, Type> OpenGenericTypes = new();
        private static readonly ConcurrentDictionary<Type, Type[]> GenericTypeParameters = new();
        private static readonly ConcurrentDictionary<Type, Type> ArrayTypes = new();
        private static readonly ConcurrentDictionary<Type, Type> EnumerableTypes = new();
        private static readonly ConcurrentDictionary<Type, Type> ReadOnlyListTypes = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type OpenGenericTypeOf(Type closedGenericType)
        {
            return OpenGenericTypes.GetOrAdd(closedGenericType, key => key.GetGenericTypeDefinition());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GenericTypeParametersOf(Type closedGenericType)
        {
            return GenericTypeParameters.GetOrAdd(closedGenericType, key => key.GetGenericArguments());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type ArrayTypeOf(Type elementType)
        {
            return ArrayTypes.GetOrAdd(elementType, key => key.MakeArrayType());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type EnumerableTypeOf(Type elementType)
        {
            return EnumerableTypes.GetOrAdd(elementType, key => typeof(IEnumerable<>).MakeGenericType(key));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type ReadOnlyListTypeOf(Type elementType)
        {
            return ReadOnlyListTypes.GetOrAdd(elementType, key => typeof(IReadOnlyList<>).MakeGenericType(key));
        }
    }
}