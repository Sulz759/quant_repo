using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using VContainer.Diagnostics;
using Object = UnityEngine.Object;

namespace VContainer.Editor.Diagnostics
{
    public sealed class VContainerInstanceTreeView : TreeView
    {
        private static int idSeed;

        public VContainerInstanceTreeView() : base(new TreeViewState())
        {
            Reload();
        }

        public DiagnosticsInfo CurrentDiagnosticsInfo { get; set; }

        private static int NextId()
        {
            return ++idSeed;
        }

        private static string Stringify(object instance)
        {
            if (ReferenceEquals(instance, null))
                return "null";
            if (instance is Object obj && obj == null)
                return "null or destroyed";
            return instance.ToString();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(NextId(), -1, "Root");
            var children = new List<TreeViewItem>();
            if (CurrentDiagnosticsInfo is DiagnosticsInfo info)
                for (var i = 0; i < info.ResolveInfo.Instances.Count; i++)
                {
                    var instance = info.ResolveInfo.Instances[i];
                    var item = new TreeViewItem(NextId(), 0,
                        $"({TypeNameHelper.GetTypeAlias(instance.GetType())}) {Stringify(instance)}");
                    AddProperties(instance, item);
                    children.Add(item);
                    SetExpanded(item.id, true);
                }

            root.children = children;
            return root;
        }

        private void AddProperties(object instance, TreeViewItem parent)
        {
            if (instance == null) return;

            var type = instance.GetType();
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var prop in props)
            {
                if (prop.PropertyType.IsSubclassOf(typeof(Object)) &&
                    prop.IsDefined(typeof(ObsoleteAttribute), true))
                    continue;

                try
                {
                    var value = prop.GetValue(instance);
                    var displayName =
                        $"{prop.Name} = ({TypeNameHelper.GetTypeAlias(prop.PropertyType)}) {Stringify(value)}";
                    parent.AddChild(new TreeViewItem(NextId(), parent.depth + 1, displayName));
                }
                catch (MissingReferenceException)
                {
                }
                catch (NotSupportedException)
                {
                }
            }

            foreach (var field in fields)
            {
                if ((field.FieldType.IsSubclassOf(typeof(Object)) &&
                     field.IsDefined(typeof(ObsoleteAttribute), true)) ||
                    field.IsDefined(typeof(CompilerGeneratedAttribute), true))
                    continue;

                try
                {
                    var value = field.GetValue(instance);
                    var displayName =
                        $"{field.Name} = ({TypeNameHelper.GetTypeAlias(field.FieldType)}) {Stringify(value)}";
                    parent.AddChild(new TreeViewItem(NextId(), parent.depth + 1, displayName));
                }
                catch (MissingReferenceException)
                {
                }
                catch (NotSupportedException)
                {
                }
            }
        }
    }
}