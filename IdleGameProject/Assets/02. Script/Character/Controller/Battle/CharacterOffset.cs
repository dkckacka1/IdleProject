using System.Linq;
using Engine.Util.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Character
{
    // 캐릭터 지정 오프셋
    public class CharacterOffset : MonoBehaviour
    {
        private Transform _hitEffectOffsetTransform;
        private Transform _createProjectileOffsetTransform;
        private Transform _fluidHealthBarOffsetTransform;
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
            _hitEffectOffsetTransform = transform.GetChildByName(HIT_EFFECT_OFFSET_NAME);
            _createProjectileOffsetTransform = transform.GetChildByName(CREATE_PROJECTILE_OFFSET_NAME);
            _fluidHealthBarOffsetTransform = transform.GetChildByName(FLUID_HEALTH_BAR_OFFSET_NAME);

            BattleTextOffsetRadius = GetComponent<Collider>().bounds.size.x / 2;
        }
    }
}
