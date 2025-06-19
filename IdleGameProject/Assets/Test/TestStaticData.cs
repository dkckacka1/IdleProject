using IdleProject.Core.GameData;
using UnityEngine;

[CreateAssetMenu(fileName = "TestStaticData", menuName = "Scriptable Objects/TestStaticData")]
public class TestStaticData : ScriptableObject
{
    public SpawnInfoData PlayerSpawnInfo;
    public SpawnInfoData EnemySpawnInfo;
}
