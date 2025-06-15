using UnityEngine;

namespace IdleProject.Battle.Spawn
{
    public class SpawnFormation : MonoBehaviour
    {
        [SerializeField] private Transform frontMiddlePosition;
        [SerializeField] private Transform frontRightPosition;
        [SerializeField] private Transform frontLeftPosition;
        [SerializeField] private Transform rearRightPosition;
        [SerializeField] private Transform rearLeftPosition;

        public Vector3 GetSpawnPosition(SpawnPositionType positionType)
        {
            return positionType switch
            {
                SpawnPositionType.FrontMiddle => frontMiddlePosition.position,
                SpawnPositionType.FrontRight => frontRightPosition.position,
                SpawnPositionType.FrontLeft => frontLeftPosition.position,
                SpawnPositionType.RearRight => rearRightPosition.position,
                SpawnPositionType.RearLeft => rearLeftPosition.position,
                _ => Vector3.zero
            };
        }
    }
}