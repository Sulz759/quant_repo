﻿using System.Threading;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks.Linq
{
    public static partial class UniTaskAsyncEnumerable
    {
        public static IUniTaskAsyncEnumerable<TResult> OfType<TResult>(this IUniTaskAsyncEnumerable<object> source)
        {
            Error.ThrowArgumentNullException(source, nameof(source));

            return new OfType<TResult>(source);
        }
    }

    internal sealed class OfType<TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly IUniTaskAsyncEnumerable<object> source;

        public OfType(IUniTaskAsyncEnumerable<object> source)
        {
            this.source = source;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _OfType(source, cancellationToken);
        }

        private class _OfType : AsyncEnumeratorBase<object, TResult>
        {
            public _OfType(IUniTaskAsyncEnumerable<object> source, CancellationToken cancellationToken)
                : base(source, cancellationToken)
            {
            }

            protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
            {
                if (sourceHasCurrent)
                {
                    if (SourceCurrent is TResult castCurent)
                    {
                        Current = castCurent;
                        result = true;
                        return true;
                    }

                    result = default;
                    return false;
                }

                result = false;
                return true;
            }
        }
    }
}