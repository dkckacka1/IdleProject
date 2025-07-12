using System;
using Engine.Core.EventBus;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character.EventGroup;
using IdleProject.Battle.Character.Skill;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Battle.UI;
using IdleProject.Core;
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{


    [System.Serializable]
    public partial class CharacterController : MonoBehaviour, IEventGroup<BattleManager>, IEnumEvent<BattleGameStateType>, IEnumEvent<BattleStateType>
    {
        [HideInInspector] public CharacterOffset offset;
        [HideInInspector] public CharacterUIController characterUI;
        [HideInInspector] public CharacterAIController characterAI;

        public CharacterBattleAnimationController AnimController;
        public CharacterSkill CharacterSkill;
        public CharacterSkill CharacterAttack;
        public StatSystem StatSystem;
        public CharacterState State;
        public EventGroup<BattleManager> BattleEventGroup;

        protected NavMeshAgent Agent;

        private BattleManager _battleManager;

        private BattleManager GetBattleManager =>
            _battleManager ??= BattleManager.Instance<BattleManager>();

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        #region 초기화 부문

        public void Initialized()
        {
            State.Initialize();
            SetEventGroup();
            SetStatValue();
            SetBattleAnimEvent();
            SetBattleSpeedTimeFactor();

            return;

            void SetStatValue()
            {
                StatSystem.SetStatValue(CharacterStatType.MovementSpeed, CharacterStatType.MovementSpeed, true);
                StatSystem.SetStatValue(CharacterStatType.AttackRange, CharacterStatType.AttackRange, true);
                StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
            }

            void SetBattleSpeedTimeFactor()
            {
                OnTimeFactorChange(GetBattleManager.GetCurrentBattleSpeed);
                AnimController.OnTimeFactorChange(_battleManager.GetCurrentBattleSpeed);
            }

            void SetEventGroup()
            {
                BattleEventGroup = new EventGroup<BattleManager>();
                BattleEventGroup.EventGroupList.Add(this);
                BattleEventGroup.EventGroupList.Add(AnimController);
                BattleEventGroup.EventGroupList.Add(characterUI);
                BattleEventGroup.EventGroupList.Add(characterAI);
                BattleEventGroup.PublishAll(GetBattleManager);
            }
        }

        #endregion

        public void Win()
        {
            AnimController.SetWin();
        }

        public void Idle()
        {
            AnimController.SetIdle();
        }

        public void OnEnumChange(BattleGameStateType type)
        {
            switch (type)
            {
                case BattleGameStateType.Play:
                    Agent.enabled = true;
                    break;
                case BattleGameStateType.Pause:
                    Agent.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void OnEnumChange(BattleStateType type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                    break;
                case BattleStateType.Battle:
                    break;
                case BattleStateType.Skill:
                    break;
                case BattleStateType.Win:
                    if(StatSystem.IsLive)
                        Win();
                    break;
                case BattleStateType.Defeat:
                    if(StatSystem.IsLive)
                        Idle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Publish(BattleManager publisher)
        {
            StatSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, ChangeMovementSpeed);
            StatSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, ChangeAttackRange);
            GetBattleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            GetBattleManager.GameStateEventBus.PublishEvent(this);
            GetBattleManager.BattleStateEventBus.PublishEvent(this);
        }

        public void UnPublish(BattleManager publisher)
        {
            StatSystem.RemoveValueChangedEvent(CharacterStatType.MovementSpeed, ChangeMovementSpeed);
            StatSystem.RemoveValueChangedEvent(CharacterStatType.AttackRange, ChangeAttackRange);
            GetBattleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            GetBattleManager.GameStateEventBus.UnPublishEvent(this);
            GetBattleManager.BattleStateEventBus.UnPublishEvent(this);
        }

        public static implicit operator Vector3(CharacterController controller) => controller.transform.position;

        public static implicit operator Transform(CharacterController controller) => controller.transform;

    }
}