using IdleProject.Core;

namespace IdleProject.Data.BehaviourData
{
    [System.Serializable]
    public abstract class BehaviourTargetingData
    {
        public bool isCheckFromTarget;
    }

    [System.Serializable]
    public class AITargetingData : BehaviourTargetingData
    {
        public CharacterAIType targetAIType;
    }
    
    [System.Serializable]
    public class RangeTargetingData : BehaviourTargetingData
    {
        public float skillRange;
        public bool distinct;
        public bool hasSelf;
        public bool isSelf;
    }

    [System.Serializable]
    public class CharacterStateTargetingData : BehaviourTargetingData
    {
        public bool isNot;
        public CharacterStateTargetType characterStateTargetType;
    }

    [System.Serializable]
    public class SingleTargetingData : BehaviourTargetingData
    {
        public SingleTargetType singleTargetType;
    }
}