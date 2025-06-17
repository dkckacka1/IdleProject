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
using Engine.Util;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using IdleProject.Core.Loading;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TestManager : SingletonMonoBehaviour<TestManager>
{
    private string GetSceneName => SceneManager.GetActiveScene().name;

    [BoxGroup("Game")]
    public bool isTestPlay;

    
    [BoxGroup("Game"), ShowIf("@this.isTestPlay")]
    public TestStaticData testData;
    
    private bool IsGamePlay => Application.isPlaying;

    [Title("SceneMove")]
    [BoxGroup("Game"),EnumToggleButtons, ShowIf("@this.IsGamePlay")]
    public SceneType moveSceneType;

    [BoxGroup("Game"),Button, ShowIf("@this.IsGamePlay")]
    private void MoveScene()
    {
        GameManager.Instance.LoadScene(moveSceneType);
    }

    [BoxGroup("Battle"), Button, ShowIf("@this.IsGamePlay && this.GetSceneName == \"Battle\"")]
    private void PlayBattle()
    {
        GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
        GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Play);
    }
}