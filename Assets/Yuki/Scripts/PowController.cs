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
        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //�v���C���[��Pow�𓊂�����͂�������
        {
            Thrown(); //������i�����j
        }
        else if (_playerCon._playMode == PlayerController.PowMode.Put)
        {
            Put(); //�u��
        }

        _playerCon._playMode = PlayerController.PowMode.Normal; //PowMode��Normal�ɖ߂�
    }

    /// <summary> ������ꂽ�Ƃ��̏��� /// </summary>
    void Thrown()
    {

    }

    /// <summary> �u���ꂽ�Ƃ��̏��� </summary>
    void Put()
    {

    }
}
