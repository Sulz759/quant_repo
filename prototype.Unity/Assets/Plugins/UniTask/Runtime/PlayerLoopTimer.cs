﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;
using UnityEditor;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
    public abstract class PlayerLoopTimer : IDisposable, IPlayerLoopItem
    {
        private readonly CancellationToken cancellationToken;
        private readonly bool periodic;
        private readonly PlayerLoopTiming playerLoopTiming;
        private readonly object state;
        private readonly Action<object> timerCallback;
        private bool isDisposed;

        private bool isRunning;
        private bool tryStop;

        protected PlayerLoopTimer(bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken,
            Action<object> timerCallback, object state)
        {
            this.periodic = periodic;
            this.playerLoopTiming = playerLoopTiming;
            this.cancellationToken = cancellationToken;
            this.timerCallback = timerCallback;
            this.state = state;
        }

        public void Dispose()
        {
            isDisposed = true;
        }

        bool IPlayerLoopItem.MoveNext()
        {
            if (isDisposed)
            {
                isRunning = false;
                return false;
            }

            if (tryStop)
            {
                isRunning = false;
                return false;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                isRunning = false;
                return false;
            }

            if (!MoveNextCore())
            {
                timerCallback(state);

                if (periodic)
                {
                    ResetCore(null);
                    return true;
                }

                isRunning = false;
                return false;
            }

            return true;
        }

        public static PlayerLoopTimer Create(TimeSpan interval, bool periodic, DelayType delayType,
            PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback,
            object state)
        {
#if UNITY_EDITOR
            // force use Realtime.
            if (PlayerLoopHelper.IsMainThread && !EditorApplication.isPlaying) delayType = DelayType.Realtime;
#endif

            switch (delayType)
            {
                case DelayType.UnscaledDeltaTime:
                    return new IgnoreTimeScalePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken,
                        timerCallback, state);
                case DelayType.Realtime:
                    return new RealtimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken,
                        timerCallback, state);
                case DelayType.DeltaTime:
                default:
                    return new DeltaTimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken,
                        timerCallback, state);
            }
        }

        public static PlayerLoopTimer StartNew(TimeSpan interval, bool periodic, DelayType delayType,
            PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback,
            object state)
        {
            var timer = Create(interval, periodic, delayType, playerLoopTiming, cancellationToken, timerCallback,
                state);
            timer.Restart();
            return timer;
        }

        /// <summary>
        ///     Restart(Reset and Start) timer.
        /// </summary>
        public void Restart()
        {
            if (isDisposed) throw new ObjectDisposedException(null);

            ResetCore(null); // init state
            if (!isRunning)
            {
                isRunning = true;
                PlayerLoopHelper.AddAction(playerLoopTiming, this);
            }

            tryStop = false;
        }

        /// <summary>
        ///     Restart(Reset and Start) and change interval.
        /// </summary>
        public void Restart(TimeSpan interval)
        {
            if (isDisposed) throw new ObjectDisposedException(null);

            ResetCore(interval); // init state
            if (!isRunning)
            {
                isRunning = true;
                PlayerLoopHelper.AddAction(playerLoopTiming, this);
            }

            tryStop = false;
        }

        /// <summary>
        ///     Stop timer.
        /// </summary>
        public void Stop()
        {
            tryStop = true;
        }

        protected abstract void ResetCore(TimeSpan? newInterval);

        protected abstract bool MoveNextCore();
    }

    internal sealed class DeltaTimePlayerLoopTimer : PlayerLoopTimer
    {
        private float elapsed;
        private int initialFrame;
        private float interval;

        public DeltaTimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming,
            CancellationToken cancellationToken, Action<object> timerCallback, object state)
            : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
        {
            ResetCore(interval);
        }

        protected override bool MoveNextCore()
        {
            if (elapsed == 0.0f)
                if (initialFrame == Time.frameCount)
                    return true;

            elapsed += Time.deltaTime;
            if (elapsed >= interval) return false;

            return true;
        }

        protected override void ResetCore(TimeSpan? interval)
        {
            elapsed = 0.0f;
            initialFrame = PlayerLoopHelper.IsMainThread ? Time.frameCount : -1;
            if (interval != null) this.interval = (float)interval.Value.TotalSeconds;
        }
    }

    internal sealed class IgnoreTimeScalePlayerLoopTimer : PlayerLoopTimer
    {
        private float elapsed;
        private int initialFrame;
        private float interval;

        public IgnoreTimeScalePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming,
            CancellationToken cancellationToken, Action<object> timerCallback, object state)
            : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
        {
            ResetCore(interval);
        }

        protected override bool MoveNextCore()
        {
            if (elapsed == 0.0f)
                if (initialFrame == Time.frameCount)
                    return true;

            elapsed += Time.unscaledDeltaTime;
            if (elapsed >= interval) return false;

            return true;
        }

        protected override void ResetCore(TimeSpan? interval)
        {
            elapsed = 0.0f;
            initialFrame = PlayerLoopHelper.IsMainThread ? Time.frameCount : -1;
            if (interval != null) this.interval = (float)interval.Value.TotalSeconds;
        }
    }

    internal sealed class RealtimePlayerLoopTimer : PlayerLoopTimer
    {
        private long intervalTicks;
        private ValueStopwatch stopwatch;

        public RealtimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming,
            CancellationToken cancellationToken, Action<object> timerCallback, object state)
            : base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
        {
            ResetCore(interval);
        }

        protected override bool MoveNextCore()
        {
            if (stopwatch.ElapsedTicks >= intervalTicks) return false;

            return true;
        }

        protected override void ResetCore(TimeSpan? interval)
        {
            stopwatch = ValueStopwatch.StartNew();
            if (interval != null) intervalTicks = interval.Value.Ticks;
        }
    }
}