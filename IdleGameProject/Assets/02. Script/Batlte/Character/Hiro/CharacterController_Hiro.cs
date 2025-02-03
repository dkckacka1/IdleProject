using UnityEngine;

namespace IdleProject.Battle.Character.Hiro
{
    public class CharacterController_Hiro : CharacterController
    {
        private AnimationEventHandler_Hiro animEventHandler;
        protected AnimationEventHandler_Hiro AnimEventHandler
        {
            get
            {
                if(animEventHandler is null)
                    animEventHandler = animController.AnimEventHandler as AnimationEventHandler_Hiro;

                return animEventHandler;
            }
        }

        protected override void SetAnimationEvent()
        {
            base.SetAnimationEvent();

            AnimEventHandler.AttackEvent += Attack;
        }


    }
}