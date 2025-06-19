using UnityEngine;
using IdleProject.Core;
using Sirenix.OdinInspector;
using IdleProject.Battle;
using Engine.Util;
using UnityEngine.SceneManagement;

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