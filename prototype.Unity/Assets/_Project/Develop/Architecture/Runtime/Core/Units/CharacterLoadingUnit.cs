using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;

namespace _Project.Develop.Architecture.Runtime.Core.Units
{
    public class CharacterLoadingUnit: ILoadUnit
    {
        public bool IsLoaded { get; private set; }

        private float _delay;
        private readonly bool _isLoadedWhenEnd;

        public CharacterLoadingUnit()
        {
            
        }
        public UniTask Load()
        {
            throw new System.NotImplementedException();
        }
    }
}