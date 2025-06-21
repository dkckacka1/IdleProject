using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data
{
    [CreateAssetMenu(fileName = "UserData", menuName = "Create/UserData")]
    public class UserData : ScriptableObject
    {
        public int PlayerLevel = 1;
        public int PlayerExp = 0;
        
        public List<string> UserHeroList;
    }
}