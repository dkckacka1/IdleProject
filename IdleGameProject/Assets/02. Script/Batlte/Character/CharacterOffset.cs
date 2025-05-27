using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class CharacterOffset : MonoBehaviour
    {
        [SerializeField, BoxGroup("CharacterOffset")]
        private Transform hitEffecOffsettTransform;
        [SerializeField, BoxGroup("CharacterOffset")]
        private Transform createProjectileOffsetTransform;

        [SerializeField, BoxGroup("UIOffsfet")]
        private Transform fluidHealthBarOffsetTransform;
        [SerializeField, BoxGroup("UIOffsfet")]
        private Transform battleTextOffsetTransform;
        [SerializeField, BoxGroup("UIOffsfet")]
        private float battleTextOffsetRadius;

        public Vector3 HitEffecOffset => hitEffecOffsettTransform.position;
        public Vector3 CreateProjectileOffset => createProjectileOffsetTransform.position;
        public Vector3 FluidHealthBarOffset => fluidHealthBarOffsetTransform.position;
        public Vector3 BattleTextOffset => battleTextOffsetTransform.position;
        public float BattleTextOffsetRadius => battleTextOffsetRadius;
    }
}
