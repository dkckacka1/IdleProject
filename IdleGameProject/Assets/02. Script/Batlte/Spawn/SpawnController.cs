using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.UI;
using IdleProject.Core;
using Zenject;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Spawn
{
    public enum SpawnPositionType
    {
        FrontMiddle,
        FrontRight,
        FrontLeft,
        RearRight,
        RearLeft,
    }

    public class SpawnController : MonoBehaviour
    {
        [Inject] private BattleManager _battleManager;
        [Inject] private BattleResourceLoader _battleResourceLoader;

        [Inject(Id = "PlayerSpawnOffset")] 
        private SpawnOffset _playerSpawn;
        
        [Inject(Id = "EnemySpawnOffset")] 
        private SpawnOffset _enemySpawn;
        
        [Inject]
        private IFactory<string, CharacterData, CharacterAIType, Task<CharacterController>> _characterControllerFactory;

        [Inject]
        private IFactory<CharacterController, CharacterData, CharacterAIType, CharacterUIController>
            _characterUIControllerFactory;

        [Inject] private IFactory<CharacterController, CharacterAIType, CharacterAIController>
            _characterAIControllerFactory;

        public async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            string characterName)
        {
            var address = _battleResourceLoader.GetCharacterControllerAddress(characterName);
            var data = await _battleResourceLoader.LoadCharacterData(characterName);

            var controller = await _characterControllerFactory.Create(address, data, aiType);
            controller.characterUI = _characterUIControllerFactory.Create(controller, data, aiType);
            controller.characterAI = _characterAIControllerFactory.Create(controller, aiType);

            _battleManager.AddCharacterController(controller);
            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType, SpawnPositionType spawnPositionType)
        {
            SpawnOffset spawnOffset = aiType == CharacterAIType.Player ? _playerSpawn : _enemySpawn;
            var spawnPosition = spawnOffset.spawn.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnOffset.spawnObject);
            character.transform.position = spawnPosition;
            character.transform.Rotate(spawnOffset.spawn.transform.rotation.eulerAngles);
        }
    }
}