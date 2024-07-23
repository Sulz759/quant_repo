using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Core.Character
{
    public class CharacterView: MonoBehaviour
    {
        private CharacterConfig _characterConfig;
        public void Initialize(CharacterConfig characterConfig)
        {
            _characterConfig = characterConfig;
            Log.Battle.D("Character is instantiate");
        }
        
    }
}