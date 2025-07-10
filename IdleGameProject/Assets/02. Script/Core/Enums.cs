namespace IdleProject.Core
{
    public enum SceneType
    {
        Lobby,
        Battle,
        Title,
    }
    
    #region Battle

    public enum BattleStateType
    {
        Ready = default,
        Battle,
        Skill,
        Win,
        Defeat,
    }

    public enum BattleGameStateType
    {
        Play = default,
        Pause
    }

    public enum BattleObjectType
    {
        Character,
        Projectile,
        Effect,
        UI
    }
    
    public enum CharacterAIType
    {
        Player,
        Enemy,
    }

    public enum PoolableType
    {
        UI,
        BattleEffect,
        Projectile,
    }

    public enum CharacterOffsetType
    {
        CharacterGround,
        ProjectileOffset,
        HitOffset,
        FluidHealthBarOffset,
    }
    
    #endregion

    #region Skill

    public enum SkillActionType
    {
        ImmediatelyAttack,
        ProjectileAttack,
        Buff
    }

    public enum SkillRangeType
    {
        InAttackRange,
        All,
        SelfRange,
        TargetRange,
    }
    
    public enum SkillTargetType
    {
        CurrentTarget,
        AllEnemy,
        AllAlly
    }

    public enum ProjectileType
    {
        Direct,
        Howitzer
    }

    public enum EffectCallTargetType
    {
        Controller,
        Target,
    }

    public enum EffectCallOffsetType
    {
        HitOffset,
        Ground,
    }

    #endregion

    public enum CharacterStatType
    {
        HealthPoint,
        ManaPoint,
        MovementSpeed,
        AttackDamage,
        AttackRange,
        AttackCoolTime,
        DefensePoint,
        CriticalPercent,
        CriticalResistance
    }
    
    public enum SpawnPositionType
    {
        FrontMiddle,
        FrontRight,
        FrontLeft,
        RearRight,
        RearLeft,
    }
    
    public enum ConsumableType
    {
        CharacterExp,
    }

    public enum EquipmentItemType
    {
        Weapon,
        Helmet,
        Armor,
        Glove,
        Boots,
        Accessory
    }

    public enum SlotPanelType
    {
        Character,
        EquipmentItem,
    }

    public enum RewardType
    {
        ConsumableItem,
        EquipmentItem
    }
}