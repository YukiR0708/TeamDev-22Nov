using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonovihair<T> : MonoBehaviour where T : MonoBehaviour
{
    protected abstract bool _dontDestroyOnLoad { get; }

    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (!instance)
                {
                    Debug.LogError(t + "ÇÕÇ†ÇËÇ‹ÇπÇÒÅB");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(this != Instance)
        {
            Destroy(this);
            return;
        }
        if(_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
