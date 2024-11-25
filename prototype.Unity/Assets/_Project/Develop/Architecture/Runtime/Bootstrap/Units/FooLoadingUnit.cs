using _Project.Develop.Architecture.Runtime.Utilities;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Bootstrap.Units
{
    public sealed class FooLoadingUnit : ILoadUnit
    {
        private readonly bool _isLoadedWhenEnd;

        private float _delay;

        public FooLoadingUnit(float delay = 0f, bool isLoadedWhenEnd = true)
        {
            _delay = delay;
            _isLoadedWhenEnd = isLoadedWhenEnd;
        }

        public bool IsLoaded { get; private set; }

        public async UniTask Load()
        {
            Log.Loading.D($"foo is loading ... {(_delay > 0 ? _delay : string.Empty)}");

            while (_delay-- > 0)
            {
                await UniTask.Delay(1000);
                Log.Loading.D($"foo is loading ... {_delay}");
            }

            await UniTask.NextFrame();

            IsLoaded = _isLoadedWhenEnd;
        }
    }
}