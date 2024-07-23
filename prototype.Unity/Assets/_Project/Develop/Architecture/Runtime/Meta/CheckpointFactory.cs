using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Meta
{
    public class CheckpointFactory: ILoadUnit
    {
        public UniTask Load()
        {
            return UniTask.CompletedTask;
        }
    }
}