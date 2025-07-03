using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Util.Extension;
using IdleProject.Core;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

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

        public SpawnPosition GetSpawnPosition(CharacterController targetCharacter)
        {
            return GetAllSpawnPosition().FirstOrDefault(position => position.Character == targetCharacter);
        }

        private List<SpawnPosition> GetAllSpawnPosition()
        {
            var result = new List<SpawnPosition>();
            
            EnumExtension.Foreach<SpawnPositionType>(type =>
            {
                result.Add(GetSpawnPosition(type));
            });

            return result;
        }
    }
}