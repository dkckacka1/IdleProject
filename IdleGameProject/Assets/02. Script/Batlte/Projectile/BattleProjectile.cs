using IdleProject.Battle.Character;
using IdleProject.Battle.Projectile.Movement;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Projectile
{
    public class  BattleProjectile : MonoBehaviour, IPoolable
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public UnityEvent hitEvent;
        [HideInInspector] public UnityEvent releaseEvent = null;

        public IProjectileMovement Movement;
        
        [SerializeField] private Transform hitTransform;
        
        private BattleManager _battleManager;
        private float _hitDistance;
        private float _distanceCheckValue;

        public void OnCreateAction()
        {
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();

            _hitDistance = hitTransform ? Vector3.Distance(transform.position, hitTransform.position) : 0;
            _distanceCheckValue = DataManager.Instance.ConstData.projectileDistanceCheckCorrectionValue;
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
            Movement = null;
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
        }

        public void SetSkillProjectile()
        {
            _battleManager.AddSkillObject(this);
            _battleManager.SkillObjectEventDic[BattleObjectType.Projectile].AddListener(OnBattleEvent);
            releaseEvent.AddListener(ReleaseAction);
            return;
            
            void ReleaseAction()
            {
                _battleManager.RemoveSkillObject(this);
                _battleManager.SkillObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
            }
        }

        private void OnBattleEvent()
        {
            Movement?.ProjectileMove(this, target, _battleManager.GetCurrentBattleDeltaTime);
            DistanceCheck();
        }

        private void DistanceCheck()
        {
            var distance = Vector3.Distance(target.position, transform.position) - _hitDistance;
            if (distance < _distanceCheckValue)
            {
                hitEvent.Invoke();
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }
    }
}