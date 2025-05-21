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

        public async UniTask<CharacterController> SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType, string characterName)
        {
            var character = await ResourcesLoader.InstantiateCharacter(characterName);
            SetCharacterPosition(character, aiType, spawnPositionType);

            var data = await ResourcesLoader.LoadCharacterData(characterName);
            await character.Initialized(data);
            character.characterAI.aiType = aiType;

            BattleManager.Instance.AddCharacterController(character, aiType);

            return character;
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType, SpawnPositionType spawnPositionType)
        {
            SpawnDatas spawnData = aiType == CharacterAIType.Playerable ? player : enemy;
            var spawnPosition = spawnData.spawn.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnData.spawnObject);
            character.transform.position = spawnPosition;
            character.transform.Rotate(spawnData.spawn.transform.rotation.eulerAngles);
        }
    }
}