using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonovihair<SceneLoader>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    public void SceneLoad(string sceneName)
    {
        StartCoroutine(LoadTime(sceneName));
    }

    public void PowResetSceneLoad(string sceneName)
    {
        GameManager.Instance.PowReset();
        StartCoroutine(LoadTime(sceneName));
    }

    IEnumerator LoadTime(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
