using UnityEngine;
using Zenject;

namespace IdleProject.Battle.Character.Skill
{
    public abstract class CharacterSkill
    {
        [Inject] 
        protected BattleManager BattleManager;
        
        public CharacterController Controller;

        public abstract void SetAnimationEvent(AnimationEventHandler eventHandler);
    }
}