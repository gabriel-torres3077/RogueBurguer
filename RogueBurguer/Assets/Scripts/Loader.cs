using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static string targetSceneIndex;

    public static void Load(string targetSceneName)
    {
        Loader.targetSceneIndex = targetSceneName;

        SceneManager.LoadScene("Loading");

    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSceneIndex);

    }
}
