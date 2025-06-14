using IdleProject.Data;using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TestData", menuName = "Scriptable Objects/TestData")]
public class TestData : Data
{
    public string testIndex;
    public override string Index => testIndex;
}
