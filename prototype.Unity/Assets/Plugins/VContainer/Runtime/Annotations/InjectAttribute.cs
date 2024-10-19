using System;
using JetBrains.Annotations;

namespace VContainer
{
    public class PreserveAttribute : Attribute
    {
    }

#if UNITY_2018_4_OR_NEWER
    [MeansImplicitUse(
        ImplicitUseKindFlags.Access |
        ImplicitUseKindFlags.Assign |
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
#endif
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Field)]
    public class InjectAttribute : PreserveAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class InjectIgnoreAttribute : Attribute
    {
    }
}