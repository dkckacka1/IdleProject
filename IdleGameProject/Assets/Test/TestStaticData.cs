using IdleProject.Core.GameData;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TestStaticData", menuName = "Scriptable Objects/TestStaticData")]
public class TestStaticData : ScriptableObject
{
    public FormationInfoData playerFormationInfo;
    public FormationInfoData enemyFormationInfo;
}