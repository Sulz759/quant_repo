﻿using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
    internal sealed class PooledDelegate<T> : ITaskPoolNode<PooledDelegate<T>>
    {
        private static TaskPool<PooledDelegate<T>> pool;

        private readonly Action<T> runDelegate;
        private Action continuation;

        private PooledDelegate<T> nextNode;

        static PooledDelegate()
        {
            TaskPool.RegisterSizeGetter(typeof(PooledDelegate<T>), () => pool.Size);
        }

        private PooledDelegate()
        {
            runDelegate = Run;
        }

        public ref PooledDelegate<T> NextNode => ref nextNode;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T> Create(Action continuation)
        {
            if (!pool.TryPop(out var item)) item = new PooledDelegate<T>();

            item.continuation = continuation;
            return item.runDelegate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Run(T _)
        {
            var call = continuation;
            continuation = null;
            if (call != null)
            {
                pool.TryPush(this);
                call.Invoke();
            }
        }
    }
}