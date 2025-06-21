using System;
using Engine.Util.Extension;
using IdleProject.Battle.AI;
using UnityEngine;

namespace IdleProject.Battle.Spawn
{
    public class SpawnFormation : MonoBehaviour
    {
        [SerializeField] private SpawnPosition frontMiddlePosition;
        [SerializeField] private SpawnPosition frontRightPosition;
        [SerializeField] private SpawnPosition frontLeftPosition;
        [SerializeField] private SpawnPosition rearRightPosition;
        [SerializeField] private SpawnPosition rearLeftPosition;

        public void SetDefaultSpawn(CharacterAIType aiType)
        {
            EnumExtension.Foreach<SpawnPositionType>(type =>
            {
                var spawnPosition = GetSpawnPosition(type);
                spawnPosition.Initialize(aiType);
            });
        }
        
        public SpawnPosition GetSpawnPosition(SpawnPositionType positionType)
        {
            return positionType switch
            {
                SpawnPositionType.FrontMiddle => frontMiddlePosition,
                SpawnPositionType.FrontRight => frontRightPosition,
                SpawnPositionType.FrontLeft => frontLeftPosition,
                SpawnPositionType.RearRight => rearRightPosition,
                SpawnPositionType.RearLeft => rearLeftPosition,
                _ => throw new ArgumentOutOfRangeException(nameof(positionType), positionType, null)
            };
        }
    }
}