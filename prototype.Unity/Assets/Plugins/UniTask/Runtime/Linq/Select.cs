﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks.Linq
{
    public static partial class UniTaskAsyncEnumerable
    {
        public static IUniTaskAsyncEnumerable<TResult> Select<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new Select<TSource, TResult>(source, selector);
        }

        public static IUniTaskAsyncEnumerable<TResult> Select<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new SelectInt<TSource, TResult>(source, selector);
        }

        public static IUniTaskAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new SelectAwait<TSource, TResult>(source, selector);
        }

        public static IUniTaskAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new SelectIntAwait<TSource, TResult>(source, selector);
        }

        public static IUniTaskAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new SelectAwaitWithCancellation<TSource, TResult>(source, selector);
        }

        public static IUniTaskAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(
            this IUniTaskAsyncEnumerable<TSource> source,
            Func<TSource, int, CancellationToken, UniTask<TResult>> selector)
        {
            Error.ThrowArgumentNullException(source, nameof(source));
            Error.ThrowArgumentNullException(selector, nameof(selector));

            return new SelectIntAwaitWithCancellation<TSource, TResult>(source, selector);
        }
    }

    internal sealed class Select<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, TResult> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _Select(source, selector, cancellationToken);
        }

        private sealed class _Select : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, TResult> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private readonly Action moveNextAction;

            private int state = -1;

            public _Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector,
                CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                Current = selector(enumerator.Current);
                                goto CONTINUE;
                            }

                            goto DONE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }

    internal sealed class SelectInt<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, int, TResult> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public SelectInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _Select(source, selector, cancellationToken);
        }

        private sealed class _Select : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, int, TResult> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private int index;
            private readonly Action moveNextAction;

            private int state = -1;

            public _Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector,
                CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                Current = selector(enumerator.Current, checked(index++));
                                goto CONTINUE;
                            }

                            goto DONE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }

    internal sealed class SelectAwait<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, UniTask<TResult>> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _SelectAwait(source, selector, cancellationToken);
        }

        private sealed class _SelectAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, UniTask<TResult>> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private UniTask<TResult>.Awaiter awaiter2;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private readonly Action moveNextAction;

            private int state = -1;

            public _SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector,
                CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                awaiter2 = selector(enumerator.Current).GetAwaiter();
                                if (awaiter2.IsCompleted) goto case 2;

                                state = 2;
                                awaiter2.UnsafeOnCompleted(moveNextAction);
                                return;
                            }

                            goto DONE;
                        case 2:
                            Current = awaiter2.GetResult();
                            goto CONTINUE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }

    internal sealed class SelectIntAwait<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, int, UniTask<TResult>> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public SelectIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _SelectAwait(source, selector, cancellationToken);
        }

        private sealed class _SelectAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, int, UniTask<TResult>> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private UniTask<TResult>.Awaiter awaiter2;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private int index;
            private readonly Action moveNextAction;

            private int state = -1;

            public _SelectAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector,
                CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                awaiter2 = selector(enumerator.Current, checked(index++)).GetAwaiter();
                                if (awaiter2.IsCompleted) goto case 2;

                                state = 2;
                                awaiter2.UnsafeOnCompleted(moveNextAction);
                                return;
                            }

                            goto DONE;
                        case 2:
                            Current = awaiter2.GetResult();
                            goto CONTINUE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }

    internal sealed class SelectAwaitWithCancellation<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, CancellationToken, UniTask<TResult>> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source,
            Func<TSource, CancellationToken, UniTask<TResult>> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _SelectAwaitWithCancellation(source, selector, cancellationToken);
        }

        private sealed class _SelectAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, CancellationToken, UniTask<TResult>> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private UniTask<TResult>.Awaiter awaiter2;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private readonly Action moveNextAction;

            private int state = -1;

            public _SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source,
                Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                awaiter2 = selector(enumerator.Current, cancellationToken).GetAwaiter();
                                if (awaiter2.IsCompleted) goto case 2;

                                state = 2;
                                awaiter2.UnsafeOnCompleted(moveNextAction);
                                return;
                            }

                            goto DONE;
                        case 2:
                            Current = awaiter2.GetResult();
                            goto CONTINUE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }

    internal sealed class SelectIntAwaitWithCancellation<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
    {
        private readonly Func<TSource, int, CancellationToken, UniTask<TResult>> selector;
        private readonly IUniTaskAsyncEnumerable<TSource> source;

        public SelectIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source,
            Func<TSource, int, CancellationToken, UniTask<TResult>> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _SelectAwaitWithCancellation(source, selector, cancellationToken);
        }

        private sealed class _SelectAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>
        {
            private readonly CancellationToken cancellationToken;
            private readonly Func<TSource, int, CancellationToken, UniTask<TResult>> selector;
            private readonly IUniTaskAsyncEnumerable<TSource> source;
            private UniTask<bool>.Awaiter awaiter;
            private UniTask<TResult>.Awaiter awaiter2;
            private IUniTaskAsyncEnumerator<TSource> enumerator;
            private int index;
            private readonly Action moveNextAction;

            private int state = -1;

            public _SelectAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source,
                Func<TSource, int, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
            {
                this.source = source;
                this.selector = selector;
                this.cancellationToken = cancellationToken;
                moveNextAction = MoveNext;
                TaskTracker.TrackActiveTask(this, 3);
            }

            public TResult Current { get; private set; }

            public UniTask<bool> MoveNextAsync()
            {
                if (state == -2) return default;

                completionSource.Reset();
                MoveNext();
                return new UniTask<bool>(this, completionSource.Version);
            }

            public UniTask DisposeAsync()
            {
                TaskTracker.RemoveTracking(this);
                return enumerator.DisposeAsync();
            }

            private void MoveNext()
            {
                try
                {
                    switch (state)
                    {
                        case -1: // init
                            enumerator = source.GetAsyncEnumerator(cancellationToken);
                            goto case 0;
                        case 0:
                            awaiter = enumerator.MoveNextAsync().GetAwaiter();
                            if (awaiter.IsCompleted) goto case 1;

                            state = 1;
                            awaiter.UnsafeOnCompleted(moveNextAction);
                            return;
                        case 1:
                            if (awaiter.GetResult())
                            {
                                awaiter2 = selector(enumerator.Current, checked(index++), cancellationToken)
                                    .GetAwaiter();
                                if (awaiter2.IsCompleted) goto case 2;

                                state = 2;
                                awaiter2.UnsafeOnCompleted(moveNextAction);
                                return;
                            }

                            goto DONE;
                        case 2:
                            Current = awaiter2.GetResult();
                            goto CONTINUE;
                        default:
                            goto DONE;
                    }
                }
                catch (Exception ex)
                {
                    state = -2;
                    completionSource.TrySetException(ex);
                    return;
                }

                DONE:
                state = -2;
                completionSource.TrySetResult(false);
                return;

                CONTINUE:
                state = 0;
                completionSource.TrySetResult(true);
            }
        }
    }
}