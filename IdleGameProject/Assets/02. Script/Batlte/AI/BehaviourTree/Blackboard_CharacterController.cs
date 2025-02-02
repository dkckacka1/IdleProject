using UnityEngine;

using Engine.AI.BehaviourTree;

using IdleProject.Battle.Character;

namespace IdleProject.Battle.AI
{
    [System.Serializable]
    public class Blackboard_CharacterController : Blackboard
    {
        public Character.CharacterController controller;
        public StatSystem stat;
    }
}