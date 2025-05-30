using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class CharacterOffset : MonoBehaviour
    {
        [SerializeField, BoxGroup("CharacterOffset")]
        private Transform hitEffectOffsetTransform;
        [SerializeField, BoxGroup("CharacterOffset")]
        private Transform createProjectileOffsetTransform;

        [SerializeField, BoxGroup("UIOffset")]
        private Transform fluidHealthBarOffsetTransform;
        [SerializeField, BoxGroup("UIOffset")]
        private Transform battleTextOffsetTransform;
        [SerializeField, BoxGroup("UIOffset")]
        private float battleTextOffsetRadius;

        public Vector3 HitEffectOffset => hitEffectOffsetTransform.position;
        public Vector3 CreateProjectileOffset => createProjectileOffsetTransform.position;
        public Vector3 FluidHealthBarOffset => fluidHealthBarOffsetTransform.position;
        public Vector3 BattleTextOffset => battleTextOffsetTransform.position;
        public float BattleTextOffsetRadius => battleTextOffsetRadius;
    }
}
