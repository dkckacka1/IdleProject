using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [HideInInspector] public UnityEvent hitEvent;
        [HideInInspector] public CharacterController target;
        [HideInInspector] public UnityEvent releaseEvent = null;

        public float projectileSpeed;
        
        private BattleManager _battleManager;
        private Vector3 _targetPosition;

        public void OnCreateAction()
        {
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();
        }

        public void OnGetAction()
        {
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].AddListener(OnBattleEvent);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            hitEvent.RemoveAllListeners();
            target = null;
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
        }

        public void SetSkillProjectile()
        {
            _battleManager.AddSkillObject(this);
            releaseEvent.AddListener(() => _battleManager.RemoveSkillObject(this));
        }

        private void OnBattleEvent()
        {
            transform.LookAt(target.HitEffectOffset);
            transform.position = Vector3.MoveTowards(transform.position, target.HitEffectOffset, projectileSpeed * _battleManager.GetCurrentBattleDeltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<CharacterController>();
            if (character is not null && character == target)
            {
                hitEvent.Invoke();
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }

        private void DistanceCheck()
        {
            var distance = Vector3.Distance(target.HitEffectOffset, this.transform.position);
            if (distance < 0.5f)
            {
                hitEvent.Invoke();
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }
    }
}