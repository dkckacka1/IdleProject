using Engine.AI.BehaviourTree;

using IdleProject.Battle.Character;
using Sirenix.OdinInspector;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI
{
    [System.Serializable]
    public class Blackboard_Character : Blackboard
    {
        public CharacterController Controller { get; private set; }
        [ShowInInspector]
        public CharacterState currentState
        {
            get
            {
                if (Application.isPlaying && Controller is not null)
                {
                    return Controller.currentState;
                }

                return CharacterState.None;
            }
        }
        [Header("CharacterStat")]
        [ShowInInspector]
        public StatSystem Stat { get; private set; }


        [Header("AI")]
        public CharacterAIController CharacterAI { get; private set; }

        [ShowInInspector]
        public CharacterController Target { get => CharacterAI is not null ? CharacterAI.target : null; set => CharacterAI.target = value; }

        public Blackboard_Character(CharacterController controller, CharacterAIController characterAI)
        {
            Controller = controller;
            Stat = controller.statSystem;
            CharacterAI = characterAI;
        }
    }
}