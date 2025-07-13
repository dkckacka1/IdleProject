using IdleProject.Core;

namespace IdleProject.Data.BehaviourData
{
    [System.Serializable]
    public abstract class BehaviourEffectData
    {
        public string effectName;
        public CharacterOffsetType offsetType;
        public bool canRotate;
    }

    [System.Serializable]
    public class LoopEffect : BehaviourEffectData
    {
        public float duration;
    }

    [System.Serializable]
    public class OneShotEffect : BehaviourEffectData
    {
        
    }
}