using _Project.Develop.Architecture.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace _Project.Develop.Architecture.Runtime.Core.Character
{
    public class CharacterFactory: ILoadUnit
    {
        private readonly ConfigContainer _configs;
        private CharacterView _prefab;
        private CharacterView _playerCharacter;
        public CharacterFactory(ConfigContainer configs)
        {
            _configs = configs;
        }
        public UniTask Load()
        {
            _prefab = Resources.Load<CharacterView>("Prefabs/Core/Trail");
            
            return UniTask.CompletedTask;
        }

        public CharacterView CreatePlayer()
        {
            var player = Object.Instantiate(_prefab, new Vector3(-6,0,-2.7f),Quaternion.identity);
            
            player.Initialize(_configs.Battle.CharacterConfig);
            return player;
        }

        public CharacterView CreateBot()
        {
            var bot = Object.Instantiate(_prefab, new Vector3(2,0,-2.7f),Quaternion.identity);
            var botConfig = _configs.Battle.BotCharacterConfig;
            
            bot.Initialize(botConfig);
            return bot;
        }
    }
}