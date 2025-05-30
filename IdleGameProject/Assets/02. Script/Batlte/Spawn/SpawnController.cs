using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Battle.UI;
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

        public async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType, string characterName)
        {
            var controller = await ResourcesLoader.InstantiateCharacter(characterName);
            var data = await ResourcesLoader.LoadCharacterData(characterName);
            controller.Initialized(data, aiType);


            BattleManager.Instance.AddCharacterController(controller);

            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType, SpawnPositionType spawnPositionType)
        {
            SpawnDatas spawnData = aiType == CharacterAIType.Player ? player : enemy;
            var spawnPosition = spawnData.spawn.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnData.spawnObject);
            character.transform.position = spawnPosition;
            character.transform.Rotate(spawnData.spawn.transform.rotation.eulerAngles);
        }


    }
}