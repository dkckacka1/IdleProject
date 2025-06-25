using IdleProject.Battle.AI;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public interface ITargetedAble
    {
        public Transform GetTransform { get; }
        public CharacterAIType GetAiType { get; }
    }

    public interface ITakeDamagedAble : ITargetedAble
    {
        public Vector3 HitEffectOffset { get; }
        public bool CanTakeDamage { get; }
        public void TakeDamage(float attackDamage);
    }
}