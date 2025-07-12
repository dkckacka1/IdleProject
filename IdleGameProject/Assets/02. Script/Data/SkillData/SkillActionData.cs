using System.Collections.Generic;
using IdleProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.SkillData
{
    [System.Serializable]
    public abstract class SkillActionData
    {
        [SerializeReference] 
        public List<SkillTargetingData> skillTargetList = new List<SkillTargetingData>();
    } 
    
    [System.Serializable]
    public class EffectSkillActionData : SkillActionData
    {
        public bool isUseCharacterEffect;
        
        [SerializeReference] 
        public SkillEffectData effectData;
    }

    [System.Serializable]
    public class AttackSkillActionData : SkillActionData
    {
        [PropertySpace(10)]
        public bool canCritical;
        public float attackValue;
        [SerializeReference]
        public OneShotEffect hitEffect;
    }
    
    [System.Serializable]
    public class ProjectileSkillActionData : SkillActionData
    {
        [PropertySpace(10)]
        public string projectileObjectName;
        public float projectileSpeed;
        public ProjectileMoveType projectileMoveType;
        public CharacterOffsetType projectileCreateOffset;
        public CharacterOffsetType projectileTargetingOffset;
            
        [FormerlySerializedAs("projectileHitExecute")] [SerializeReference]
        public List<SkillActionData> projectileOnHitAction;
    }

    [System.Serializable]
    public class BuffSkillActionData : SkillActionData
    {
        [PropertySpace(10)] 
        public CharacterStatType buffStatType;
        public float value;
        public float duration;
        
        [SerializeReference]
        public LoopEffect buffEffect;
    }

    [System.Serializable]
    public abstract class SkillEffectData
    {
        public string effectName;
        public CharacterOffsetType offsetType;
        public bool canRotate;
    }

    [System.Serializable]
    public class LoopEffect : SkillEffectData
    {
        public float duration;
    }

    [System.Serializable]
    public class OneShotEffect : SkillEffectData
    {
        
    }

    [System.Serializable]
    public abstract class SkillTargetingData
    {
        public bool isCheckFromTarget;
    }

    [System.Serializable]
    public class AITargetingData : SkillTargetingData
    {
        public CharacterAIType targetAIType;
    }
    
    [System.Serializable]
    public class RangeTargetingData : SkillTargetingData
    {
        public float skillRange;
        public bool distinct;
        public bool hasSelf;
        public bool isSelf;
    }

    [System.Serializable]
    public class CharacterStateTargetingData : SkillTargetingData
    {
        public enum CharacterStateTargetType
        {
            IsLive,
        }
        
        public bool isNot;
        public CharacterStateTargetType characterStateTargetType;
    }

    [System.Serializable]
    public class SingleTargetingData : SkillTargetingData
    {
        public enum SingleTargetType
        {
            CurrentTarget,
            NealyController
        }

        public SingleTargetType singleTargetType;
    }
}