using System;
using System.Linq;
using Engine.Util.Extension;
using IdleProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    // 캐릭터 지정 오프셋
    public class CharacterOffset : MonoBehaviour
    {
        private Transform _hitEffectOffsetTransform;
        private Transform _createProjectileOffsetTransform;
        private Transform _fluidHealthBarOffsetTransform;
        public float BattleTextOffsetRadius { get; private set; }

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

        public Transform GetOffsetTransform(CharacterOffsetType offsetType) => offsetType switch
        {
            CharacterOffsetType.CharacterGround => transform,
            CharacterOffsetType.ProjectileOffset => _createProjectileOffsetTransform ? _createProjectileOffsetTransform : transform,
            CharacterOffsetType.HitOffset => _hitEffectOffsetTransform ? _hitEffectOffsetTransform: transform,
            CharacterOffsetType.FluidHealthBarOffset => _fluidHealthBarOffsetTransform? _fluidHealthBarOffsetTransform: transform,
            _ => throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null)
        };
    }
}
