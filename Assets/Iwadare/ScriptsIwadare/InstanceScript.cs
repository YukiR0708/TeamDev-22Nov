using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceScript : MonoBehaviour
{
    public void InstanceLoad(string sceneName)
    {
        SceneLoader.Instance.SceneLoad(sceneName);
    }

    public void InstanceResetLoad(string sceneName)
    {
        SceneLoader.Instance.PowResetSceneLoad(sceneName);
    }
}
