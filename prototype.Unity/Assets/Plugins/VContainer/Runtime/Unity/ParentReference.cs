using System;
using UnityEngine;

namespace VContainer.Unity
{
    [Serializable]
    public struct ParentReference : ISerializationCallbackReceiver
    {
        [SerializeField] public string TypeName;

        [NonSerialized] public LifetimeScope Object;

        private ParentReference(Type type)
        {
            Type = type;
            TypeName = type.FullName;
            Object = null;
        }

        public Type Type { get; private set; }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            TypeName = Type?.FullName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(TypeName))
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type = assembly.GetType(TypeName);
                    if (Type != null)
                        break;
                }
        }

        public static ParentReference Create<T>() where T : LifetimeScope
        {
            return new ParentReference(typeof(T));
        }
    }
}