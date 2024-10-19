using System;

namespace VContainer.Internal
{
    internal sealed class TypedParameter : IInjectParameter
    {
        public readonly Type Type;
        public readonly object Value;

        public TypedParameter(Type type, object value)
        {
            Type = type;
            Value = value;
        }

        public bool Match(Type parameterType, string _)
        {
            return parameterType == Type;
        }

        public object GetValue(IObjectResolver _)
        {
            return Value;
        }
    }

    internal sealed class FuncTypedParameter : IInjectParameter
    {
        public readonly Func<IObjectResolver, object> Func;
        public readonly Type Type;

        public FuncTypedParameter(Type type, Func<IObjectResolver, object> func)
        {
            Type = type;
            Func = func;
        }

        public bool Match(Type parameterType, string _)
        {
            return parameterType == Type;
        }

        public object GetValue(IObjectResolver resolver)
        {
            return Func(resolver);
        }
    }

    internal sealed class NamedParameter : IInjectParameter
    {
        public readonly string Name;
        public readonly object Value;

        public NamedParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public bool Match(Type _, string parameterName)
        {
            return parameterName == Name;
        }

        public object GetValue(IObjectResolver _)
        {
            return Value;
        }
    }

    internal sealed class FuncNamedParameter : IInjectParameter
    {
        public readonly Func<IObjectResolver, object> Func;
        public readonly string Name;

        public FuncNamedParameter(string name, Func<IObjectResolver, object> func)
        {
            Name = name;
            Func = func;
        }

        public bool Match(Type _, string parameterName)
        {
            return parameterName == Name;
        }

        public object GetValue(IObjectResolver resolver)
        {
            return Func(resolver);
        }
    }
}