namespace IdleProject.Core
{
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
    #endregion

    public enum CharacterStatType
    {
        HealthPoint,
        ManaPoint,
        MovementSpeed,
        AttackDamage,
        AttackRange,
        AttackCoolTime,
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
        Skill
    }
}