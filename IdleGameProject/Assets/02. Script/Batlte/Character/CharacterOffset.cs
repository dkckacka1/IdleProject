using System.Linq;
using Engine.Util.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    // 캐릭터 지정 오프셋
    public class CharacterOffset : MonoBehaviour
    {
        [SerializeField] private Transform _hitEffectOffsetTransform;
        [SerializeField] private Transform _createProjectileOffsetTransform;
        [SerializeField] private Transform _fluidHealthBarOffsetTransform;
        public float BattleTextOffsetRadius { get; private set; }

        public Vector3 GetHitEffectPosition =>
            _hitEffectOffsetTransform ? _hitEffectOffsetTransform.position : transform.position;

        public Vector3 GetProjectilePosition => _createProjectileOffsetTransform
            ? _createProjectileOffsetTransform.position
            : transform.position;

        public Vector3 GetFluidHealthBarPosition => _fluidHealthBarOffsetTransform
            ? _fluidHealthBarOffsetTransform.position
            : transform.position;

        private const string HIT_EFFECT_OFFSET_NAME = "HitEffectOffset";
        private const string CREATE_PROJECTILE_OFFSET_NAME = "CreateProjectileOffset";
        private const string FLUID_HEALTH_BAR_OFFSET_NAME = "FluidHealthBarOffset";
        
        public void Initialized()
        {
            Debug.Log(name);
            
            var hitOffset = transform.GetChildByName(HIT_EFFECT_OFFSET_NAME);
            var createOffset = transform.GetChildByName(CREATE_PROJECTILE_OFFSET_NAME);
            var fluidOffset = transform.GetChildByName(FLUID_HEALTH_BAR_OFFSET_NAME);
            
            _hitEffectOffsetTransform = hitOffset;
            _createProjectileOffsetTransform = createOffset;
            _fluidHealthBarOffsetTransform = fluidOffset;

            BattleTextOffsetRadius = GetComponent<Collider>().bounds.size.x / 2;
        }
    }
}
