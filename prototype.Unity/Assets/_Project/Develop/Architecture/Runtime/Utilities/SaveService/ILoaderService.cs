using System;

namespace _Project.Develop.Architecture.Runtime.Utilities.SaveService
{
    public interface ILoaderService
    {
        void Load<T>(string key, Action<T> callback);
    }
}