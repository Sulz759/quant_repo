using System;

namespace VContainer.Internal
{
    internal readonly struct BuilderCallbackDisposable : IDisposable
    {
        private readonly Action<IObjectResolver> callback;
        private readonly IObjectResolver container;

        public BuilderCallbackDisposable(Action<IObjectResolver> callback, IObjectResolver container)
        {
            this.callback = callback;
            this.container = container;
        }

        public void Dispose()
        {
            callback.Invoke(container);
        }
    }
}