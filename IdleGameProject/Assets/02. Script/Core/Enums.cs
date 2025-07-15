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
        Ally,
        Enemy,
    }

    public enum PoolableType
    {
        UI,
        BattleEffect,
        Projectile,
    }

    #endregion

    #region Behaviour

    public enum ProjectileMoveType
    {
        Direct,
        Howitzer
    }
    
    public enum CharacterOffsetType
    {
        CharacterGround,
        ProjectileOffset,
        HitOffset,
        FluidHealthBarOffset,
    }
    
    public enum SingleTargetType
    {
        Self,
        CurrentTarget,
        NealyController,
        FarthestController,
    }
    
    public enum CharacterStateTargetType
    {
        IsLive,
        IsInAttackRange
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
}