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
            var controller = await ResourcesLoader.GetCharacter(characterName);
            //var data = await ResourcesLoader.GetCharacterData(characterName);

            //SpawnDatas datas = aiType == CharacterAIType.Playerable ? player : enemy;
            //var spawnPosition = datas.spawn.GetSpawnPosition(spawnPositionType);

            //CharacterController character = Instantiate<CharacterController>(controller, spawnPosition, datas.spawn.transform.rotation, datas.spawnObject);
            //character.SetCharacterData(data);
            //character.GetComponent<CharacterAIController>().aiType = aiType;
        }
    }
}