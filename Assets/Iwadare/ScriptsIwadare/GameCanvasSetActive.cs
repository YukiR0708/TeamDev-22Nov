using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasSetActive : MonoBehaviour
{
    GameObject _gameCanvas;

    private void Awake()
    {
        _gameCanvas = GameObject.Find("GameCanvas");
    }

    private void OnEnable()
    {
        GameManager.Instance._playGame = false;
        _gameCanvas.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    GameManager.Instance._playGame = true;
    //    _gameCanvas.SetActive(true);
    //}
}
