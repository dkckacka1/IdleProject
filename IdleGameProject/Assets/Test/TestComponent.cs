using UnityEngine;
using IdleProject.Battle.Character;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using IdleProject.Core;
using Sirenix.OdinInspector;
using IdleProject.Battle;
using IdleProject.Battle.AI;
using IdleProject.Core.ObjectPool;
using UnityEngine.UI;
using Engine.Core.Addressable;
using IdleProject.Battle.Projectile;
using System.Collections.Generic;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using Engine.Core.Time;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using IdleProject.Core.Loading;

public class TestComponent : MonoBehaviour
{
    [Button]
    private void Test()
    {
        BattleManager.Instance.spawnController.SpawnCharacter(CharacterAIType.Player, SpawnPositionType.FrontLeft,
            "Hiro");
        BattleManager.Instance.spawnController.SpawnCharacter(CharacterAIType.Player, SpawnPositionType.FrontMiddle,
            "Eli");
        BattleManager.Instance.spawnController.SpawnCharacter(CharacterAIType.Enemy, SpawnPositionType.FrontMiddle,
            "GoblinMale");
    }

    [Button]
    private void Test2()
    {
        BattleManager.Instance.BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
        BattleManager.Instance.GameStateEventBus.ChangeEvent(GameStateType.Play);
    }
}