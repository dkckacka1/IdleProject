using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Effect;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using IdleProject.Battle.Projectile;
using System;
using UnityEngine;
using UnityEngine.AI;
using IdleProject.Battle.Character.Skill;
using Engine.Core;
using IdleProject.Data;
using UnityEngine.Serialization;

namespace IdleProject.Battle.Character
{
    public struct CharacterState
    {
        public bool CanMove;
        public bool CanAttack;
        public bool IsDead;

        public void Initialize()
        {
            CanMove = true;
            CanAttack = true;
            IsDead = false;
        }
    }

    [System.Serializable]
    public partial class CharacterController : MonoBehaviour
    {
        [HideInInspector] public CharacterOffset offset;
        [HideInInspector] public CharacterUIController characterUI;
        [HideInInspector] public CharacterAIController characterAI;
        
        public AnimationController AnimController;
        public CharacterSkill CharacterSkill;
        public StatSystem StatSystem;
        public CharacterState State;
        
        protected NavMeshAgent Agent;

        public Func<BattleEffect> GetAttackHitEffect;
        public Func<BattleEffect> GetSkillHitEffect;
        public Func<BattleProjectile> GetAttackProjectile;
        public Func<BattleProjectile> GetSkillProjectile;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        #region 초기화 부문
        public virtual void PublishEvent()
        {
            PublishStatModifyEvent();
            PublishAnimationEvent();
            PublishUIEvent();
            PublishAIEvent();
        }

        public void InitStats()
        {
            State.Initialize();
            StatSystem.SetStatValue(CharacterStatType.MovementSpeed, CharacterStatType.MovementSpeed, true);
            StatSystem.SetStatValue(CharacterStatType.AttackRange, CharacterStatType.AttackRange, true);
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
        }
        
        private void PublishAnimationEvent()
        {
            AnimController.OnTimeFactorChange(BattleManager.GetCurrentBattleSpeed);
            
            BattleManager.GetChangeBattleSpeedEvent.AddListener(AnimController.OnTimeFactorChange);
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.PublishEvent(AnimController);
            SetBattleAnimEvent();
        }
        
        private void PublishUIEvent()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().BattleObjectEventDic[BattleObjectType.UI].AddListener(characterUI.OnBattleUIEvent);
        }

        private void PublishAIEvent()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().BattleObjectEventDic[BattleObjectType.Character].AddListener(characterAI.OnBatteEvent);
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

        public static implicit operator Vector3(CharacterController controller) => controller.transform.position;

        public static implicit operator Transform(CharacterController controller) => controller.transform;
    }
}