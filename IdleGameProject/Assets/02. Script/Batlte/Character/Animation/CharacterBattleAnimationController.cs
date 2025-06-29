using System;
using Engine.Core.EventBus;
using IdleProject.Battle.Character.EventGroup;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class CharacterBattleAnimationController : IEnumEvent<BattleGameStateType>, IEventGroup<BattleManager>
    {
        public AnimationEventHandler AnimEventHandler { get; private set; }

        private readonly Animator _animator;

        private readonly int _skillAnimHash = Animator.StringToHash("Skill");
        private readonly int _attackAnimHash = Animator.StringToHash("Attack");
        private readonly int _deathAnimHash = Animator.StringToHash("Death");
        private readonly int _moveAnimHash = Animator.StringToHash("Move");
        private readonly int _idleAnimHash = Animator.StringToHash("Idle");
        private readonly int _winAnimHash = Animator.StringToHash("Win");

        public CharacterBattleAnimationController(Animator animator, AnimationEventHandler animEventHandler)
        {
            _animator = animator;
            AnimEventHandler = animEventHandler;
        }

        public void SetAnimationController(RuntimeAnimatorController animationController)
        {
            _animator.runtimeAnimatorController = animationController;
            _animator.Rebind();
        }

        public void SetSkill()
        {
            _animator.SetTrigger(_skillAnimHash);
        }

        public void SetAttack()
        {
            ResetAnimation();
            _animator.SetTrigger(_attackAnimHash);
        }

        public void SetDeath()
        {
            ResetAnimation();
            _animator.SetTrigger(_deathAnimHash);
        }

        public void SetMove()
        {
            _animator.SetTrigger(_moveAnimHash);
        }

        public void SetIdle()
        {
            _animator.SetTrigger(_idleAnimHash);
        }

        public void SetWin()
        {
            ResetAnimation();
            _animator.SetTrigger(_winAnimHash);
        }

        private void ResetAnimation()
        {
            foreach (var trigger in _animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    _animator.ResetTrigger(trigger.name);
                }
            }
        }
        
        public void SetAnimationSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public void OnTimeFactorChange(float timeFactor)
        {
            SetAnimationSpeed(timeFactor);
        }

        public void OnEnumChange(BattleGameStateType type)
        {
            _animator.enabled = type switch
            {
                BattleGameStateType.Play => true,
                BattleGameStateType.Pause => false,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void Publish(BattleManager publisher)
        {
            publisher.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            publisher.GameStateEventBus.PublishEvent(this);
        }

        public void UnPublish(BattleManager publisher)
        {
            publisher.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            publisher.GameStateEventBus.UnPublishEvent(this);
        }
    }
}