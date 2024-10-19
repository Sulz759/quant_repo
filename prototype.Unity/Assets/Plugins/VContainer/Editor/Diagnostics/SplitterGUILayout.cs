using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VContainer.Editor.Diagnostics
{
    // reflection call of UnityEditor.SplitterGUILayout
    internal static class SplitterGUILayout
    {
        private static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                                     BindingFlags.Instance | BindingFlags.Static;

        private static readonly Lazy<Type> SplitterStateType = new(() =>
        {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.SplitterState");
            return type;
        });

        private static readonly Lazy<ConstructorInfo> SplitterStateCtor = new(() =>
        {
            var type = SplitterStateType.Value;
            return type.GetConstructor(flags, null, new[] { typeof(float[]), typeof(int[]), typeof(int[]) }, null);
        });

        private static readonly Lazy<Type> SplitterGUILayoutType = new(() =>
        {
            var type = typeof(EditorWindow).Assembly.GetTypes()
                .First(x => x.FullName == "UnityEditor.SplitterGUILayout");
            return type;
        });

        private static readonly Lazy<MethodInfo> BeginVerticalSplitInfo = new(() =>
        {
            var type = SplitterGUILayoutType.Value;
            return type.GetMethod("BeginVerticalSplit", flags, null,
                new[] { SplitterStateType.Value, typeof(GUILayoutOption[]) }, null);
        });

        private static readonly Lazy<MethodInfo> EndVerticalSplitInfo = new(() =>
        {
            var type = SplitterGUILayoutType.Value;
            return type.GetMethod("EndVerticalSplit", flags, null, Type.EmptyTypes, null);
        });

        private static readonly Lazy<MethodInfo> BeginHorizontalSplitInfo = new(() =>
        {
            var type = SplitterGUILayoutType.Value;
            return type.GetMethod("BeginHorizontalSplit", flags, null,
                new[] { SplitterStateType.Value, typeof(GUILayoutOption[]) }, null);
        });

        private static readonly Lazy<MethodInfo> EndHorizontalSplitInfo = new(() =>
        {
            var type = SplitterGUILayoutType.Value;
            return type.GetMethod("EndHorizontalSplit", flags, null, Type.EmptyTypes, null);
        });

        public static object CreateSplitterState(float[] relativeSizes, int[] minSizes, int[] maxSizes)
        {
            return SplitterStateCtor.Value.Invoke(new object[] { relativeSizes, minSizes, maxSizes });
        }

        public static void BeginVerticalSplit(object splitterState, params GUILayoutOption[] options)
        {
            BeginVerticalSplitInfo.Value.Invoke(null, new[] { splitterState, options });
        }

        public static void EndVerticalSplit()
        {
            EndVerticalSplitInfo.Value.Invoke(null, Array.Empty<object>());
        }

        public static void BeginHorizontalSplit(object splitterState, params GUILayoutOption[] options)
        {
            BeginHorizontalSplitInfo.Value.Invoke(null, new[] { splitterState, options });
        }

        public static void EndHorizontalSplit()
        {
            EndHorizontalSplitInfo.Value.Invoke(null, Array.Empty<object>());
        }
    }
}