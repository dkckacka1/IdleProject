namespace IdleProject.Core
{
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
}