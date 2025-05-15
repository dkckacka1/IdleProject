using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Core;
using UnityEngine;

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

    [System.Serializable]
    public struct SpawnDatas
    {
        public Spawn spawn;
        public Transform spawnObject;
    }

    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private SpawnDatas player;
        [SerializeField] private SpawnDatas enemy;

        public async UniTaskVoid SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType, string characterName)
        {
            var character = await ResourcesLoader.InstantiateCharacter(characterName);
            SetCharacterPosition(character, aiType, spawnPositionType);

            var data = await ResourcesLoader.LoadCharacterData(characterName);
            character.SetCharacterData(data);

            var ai = character.gameObject.GetComponent<CharacterAIController>();
            ai.aiType = aiType;

            BattleManager.Instance.AddCharacterController(character, aiType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType, SpawnPositionType spawnPositionType)
        {
            SpawnDatas spawnData = aiType == CharacterAIType.Playerable ? player : enemy;

            var spawnPosition = spawnData.spawn.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnData.spawnObject);
            character.transform.position = spawnPosition;
        }
    }
}