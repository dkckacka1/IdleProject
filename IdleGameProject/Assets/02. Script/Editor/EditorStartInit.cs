using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class EditorStartInit
{
    static EditorStartInit()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Test")
        {
            EditorSceneManager.playModeStartScene = null;
            return;
        }
        
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}