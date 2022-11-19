using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowController : MonoBehaviour
{
    PlayerController _playerCon;

    private void Awake()
    {
        _playerCon = GameObject.Find("Player").GetComponent<PlayerController>();

    }
    void Start()
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //プレイヤーがPowを投げる入力をしたら
        {
            Thrown(); //投げる（物理）
        }
        else if (_playerCon._playMode == PlayerController.PowMode.Put)
        {
            Put(); //置く
        }

        _playerCon._playMode = PlayerController.PowMode.Normal; //PowModeをNormalに戻す
    }

    /// <summary> 投げられたときの処理 /// </summary>
    void Thrown()
    {

    }

    /// <summary> 置かれたときの処理 </summary>
    void Put()
    {

    }
}
