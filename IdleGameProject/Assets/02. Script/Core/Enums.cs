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
    
    public enum ResourceType
    {
        Character,
        UI,
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

    public enum ProjectileMoveType
    {
        PositioningType, // 위치 지정
        ChasingType, // 대상 추적
    }

    public enum ProjectileCheckType
    {
        CollisionTriggerCheck, // 콜리전 체크
        ReachedType, // 도착시
    }
    
    #endregion

    #region Skill

    public enum SkillType
    {
        Attack,
        Buff
    }

    public enum SkillRangeType
    {
        Single,
        Multi,
        SelfRange,
        TargetRange,
    }
    
    public enum SkillTargetType
    {
        NealyTarget,
        Ally,
    }

    public enum ProjectileType
    {
        Direct,
        Howitzer
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