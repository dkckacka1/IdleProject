using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class StatSystem
    {
        public StatData stat;

        public StatSystem(CharacterData data)
        {
            stat = data.stat;
        }
    }
}