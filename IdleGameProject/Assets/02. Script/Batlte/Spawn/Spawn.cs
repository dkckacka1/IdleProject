using UnityEngine;

namespace IdleProject.Battle.Spawn
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private Transform frontMiddlePosition;
        [SerializeField] private Transform frontRightPosition;
        [SerializeField] private Transform frontLeftPosition;
        [SerializeField] private Transform rearRightPosition;
        [SerializeField] private Transform rearLeftPosition;

        public Vector3 GetSpawnPosition(SpawnPositionType positionType)
        {
            switch (positionType)
            {
                case SpawnPositionType.FrontMiddle:
                    return frontMiddlePosition.position;
                case SpawnPositionType.FrontRight:
                    return frontRightPosition.position;
                case SpawnPositionType.FrontLeft:
                    return frontLeftPosition.position;
                case SpawnPositionType.RearRight:
                    return rearRightPosition.position;
                case SpawnPositionType.RearLeft:
                    return rearLeftPosition.position;
            }

            return Vector3.zero;
        }
    }
}