using UnityEngine;

namespace IdleProject.Battle.Character.Hiro
{
    public class CharacterController_Hiro : CharacterController
    {
        protected AnimationEventHandler_Hiro AnimEventHandler
        {
            get
            {
                return animEventHandler as AnimationEventHandler_Hiro;
            }
        }

        protected override void SetAnimationEvent()
        {
            base.SetAnimationEvent();

            AnimEventHandler.AttackEvent += Attack;
        }


    }
}