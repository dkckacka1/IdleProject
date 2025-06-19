using IdleProject.Core.GameData;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TestStaticData", menuName = "Scriptable Objects/TestStaticData")]
public class TestStaticData : ScriptableObject
{
    [FormerlySerializedAs("PlayerSpawnInfo")] public FormationInfo playerFormationInfo;
    [FormerlySerializedAs("EnemySpawnInfo")] public FormationInfo enemyFormationInfo;
}
