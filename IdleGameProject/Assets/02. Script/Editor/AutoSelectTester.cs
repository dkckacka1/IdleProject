using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AutoSelectTester
{
    static AutoSelectTester()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var testerObject = GameObject.Find("Tester");
        if (testerObject)
        {
            Selection.activeGameObject = testerObject;

            SceneView.lastActiveSceneView.FrameSelected();
        }
    }
}