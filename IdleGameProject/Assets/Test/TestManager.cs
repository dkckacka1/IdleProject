using System;
using Engine.Util;
using IdleProject.Battle;
using IdleProject.Core;
using IdleProject.Data.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : SingletonMonoBehaviour<TestManager>
{
    private string GetSceneName => SceneManager.GetActiveScene().name;

    [BoxGroup("Game")]
    public bool isTestPlay;
    
    [BoxGroup("Game"), ShowIf("@this.isTestPlay")]
    public PlayerData testPlayerData;
    
    private bool IsGamePlay => Application.isPlaying;

    [Title("SceneMove")]
    [BoxGroup("Game"),EnumToggleButtons, ShowIf("@this.IsGamePlay")]
    public SceneType moveSceneType;

    private void Start()
    {
        if (Debug.isDebugBuild is false)
        {
            Destroy(gameObject);
        }
    }
    

    [BoxGroup("Game"),Button, ShowIf("@this.IsGamePlay")]
    private void MoveScene()
    {
        GameManager.Instance.LoadScene(moveSceneType);
    }

    [BoxGroup("Battle"), Button, ShowIf("@this.IsGamePlay && this.GetSceneName == \"Battle\"")]
    private void PlayBattle()
    {
        GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
        GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(BattleGameStateType.Play);
    }
}