﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
    // UniTask has no scheduler like TaskScheduler.
    // Only handle unobserved exception.

    public static class UniTaskScheduler
    {
        /// <summary>
        ///     Propagate OperationCanceledException to UnobservedTaskException when true. Default is false.
        /// </summary>
        public static bool PropagateOperationCanceledException = false;

        public static event Action<Exception> UnobservedTaskException;

        internal static void PublishUnobservedTaskException(Exception ex)
        {
            if (ex != null)
            {
                if (!PropagateOperationCanceledException && ex is OperationCanceledException) return;

                if (UnobservedTaskException != null)
                {
#if UNITY_2018_3_OR_NEWER
                    if (!DispatchUnityMainThread ||
                        Thread.CurrentThread.ManagedThreadId == PlayerLoopHelper.MainThreadId)
                        // allows inlining call.
                        UnobservedTaskException.Invoke(ex);
                    else
                        // Post to MainThread.
                        PlayerLoopHelper.UnitySynchronizationContext.Post(handleExceptionInvoke, ex);
#else
                    UnobservedTaskException.Invoke(ex);
#endif
                }
                else
                {
#if UNITY_2018_3_OR_NEWER
                    string msg = null;
                    if (UnobservedExceptionWriteLogType != LogType.Exception) msg = "UnobservedTaskException: " + ex;
                    switch (UnobservedExceptionWriteLogType)
                    {
                        case LogType.Error:
                            Debug.LogError(msg);
                            break;
                        case LogType.Assert:
                            Debug.LogAssertion(msg);
                            break;
                        case LogType.Warning:
                            Debug.LogWarning(msg);
                            break;
                        case LogType.Log:
                            Debug.Log(msg);
                            break;
                        case LogType.Exception:
                            Debug.LogException(ex);
                            break;
                    }
#else
                    Console.WriteLine("UnobservedTaskException: " + ex.ToString());
#endif
                }
            }
        }

#if UNITY_2018_3_OR_NEWER

        /// <summary>
        ///     Write log type when catch unobserved exception and not registered UnobservedTaskException. Default is Exception.
        /// </summary>
        public static LogType UnobservedExceptionWriteLogType = LogType.Exception;

        /// <summary>
        ///     Dispatch exception event to Unity MainThread. Default is true.
        /// </summary>
        public static bool DispatchUnityMainThread = true;

        // cache delegate.
        private static readonly SendOrPostCallback handleExceptionInvoke = InvokeUnobservedTaskException;

        private static void InvokeUnobservedTaskException(object state)
        {
            UnobservedTaskException((Exception)state);
        }
#endif
    }
}