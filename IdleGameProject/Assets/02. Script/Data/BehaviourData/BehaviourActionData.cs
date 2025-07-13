using System.Collections.Generic;
using IdleProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Data.BehaviourData
{
    [System.Serializable]
    public abstract class BehaviourActionData
    {
        [SerializeReference] 
        public List<BehaviourTargetingData> skillTargetList = new List<BehaviourTargetingData>();
    } 
    
    [System.Serializable]
    public class EffectBehaviourActionData : BehaviourActionData
    {
        public bool isUseCharacterEffect;
        
        [SerializeReference] 
        public BehaviourEffectData effectData;
    }

    [System.Serializable]
    public class AttackBehaviourActionData : BehaviourActionData
    {
        [PropertySpace(10)]
        public bool canCritical;
        public float attackValue;
        [SerializeReference]
        public OneShotEffect hitEffect;
    }
    
    [System.Serializable]
    public class ProjectileBehaviourActionData : BehaviourActionData
    {
        [PropertySpace(10)]
        public string projectileObjectName;
        public float projectileSpeed;
        public CharacterOffsetType projectileCreateOffset;
        public CharacterOffsetType projectileTargetingOffset;
        public ProjectileMoveType projectileMoveType;
        [ShowIf("projectileMoveType", ProjectileMoveType.Howitzer)]
        public float arcHeight;
            
        [SerializeReference]
        public List<BehaviourActionData> projectileOnHitAction;
    }

    [System.Serializable]
    public class BuffBehaviourActionData : BehaviourActionData
    {
        [PropertySpace(10)] 
        public CharacterStatType buffStatType;
        public float value;
        public float duration;
        
        [SerializeReference]
        public LoopEffect buffEffect;
    }

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
        public enum CharacterStateTargetType
        {
            IsLive,
        }
        
        public bool isNot;
        public CharacterStateTargetType characterStateTargetType;
    }

    [System.Serializable]
    public class SingleTargetingData : BehaviourTargetingData
    {
        public enum SingleTargetType
        {
            CurrentTarget,
            NealyController,
            FarthestController,
        }

        public SingleTargetType singleTargetType;
    }
}