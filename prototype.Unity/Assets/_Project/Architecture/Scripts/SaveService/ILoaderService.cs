using System;

namespace _Project.Architecture
{
    public interface ILoaderService
    {
        void Load<T>(string key, Action<T> callback);
    }
}