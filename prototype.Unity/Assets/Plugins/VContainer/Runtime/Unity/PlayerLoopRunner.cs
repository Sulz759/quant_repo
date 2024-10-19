using VContainer.Internal;

namespace VContainer.Unity
{
    internal interface IPlayerLoopItem
    {
        bool MoveNext();
    }

    internal sealed class PlayerLoopRunner
    {
        private readonly FreeList<IPlayerLoopItem> runners = new(16);

        private int running;

        public void Dispatch(IPlayerLoopItem item)
        {
            runners.Add(item);
        }

        public void Run()
        {
            var span =
#if NETSTANDARD2_1
                runners.AsSpan();
#else
                runners;
#endif
            for (var i = 0; i < span.Length; i++)
            {
                var item = span[i];
                if (item != null)
                {
                    var continued = item.MoveNext();
                    if (!continued) runners.RemoveAt(i);
                }
            }
        }
    }
}