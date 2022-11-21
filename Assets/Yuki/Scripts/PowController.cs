using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowController : MonoBehaviour
{
    PlayerController _playerCon;
    Rigidbody2D _powRb = default;


    private void Awake()
    {
        _playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        _powRb = gameObject.GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //�������[�h��������
        {
            Thrown(); //������i�����j
        }
        else if (_playerCon._playMode == PlayerController.PowMode.Put)
        {
            Put(); //�u��
        }

    }

    /// <summary> ������ꂽ�Ƃ��̏��� /// </summary>
    void Thrown()
    {
        _powRb.bodyType = RigidbodyType2D.Dynamic;  //�����̂Ƃ��͕�������������

    }

    /// <summary> �u���ꂽ�Ƃ��̏��� </summary>
    void Put()
    {
        Debug.Log("�u�������Ƃ̏���");
        _powRb.bodyType = RigidbodyType2D.Static;   //�u���Ƃ��͌Œ肷��
        _playerCon._playMode = PlayerController.PowMode.Normal; //PowMode��Normal�ɖ߂�
    }

    private void OnCollisionEnter(Collision other)  //���ɓ��������疼�O��Ԃ�
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw)
        {
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow")) //�������[�h�Œn�ʂ��ǂɂ���������
            {
                Debug.Log("���������Ƃ̏���");
                //SE�炵�ĉ�ʂ�U��������B
                //��ʓ��̃U�R�G�������擾���ē|��

                


            }
            else if (other.gameObject.CompareTag("ShakeDown") || other.gameObject.CompareTag("HitDown")) //�U�R�����G�ɂ���������
            {
                //SE�炵�ĉ�ʂ�U��������B
                //���������G�Ɖ�ʓ��̃U�R�G���|���
            }

            _playerCon._playMode = PlayerController.PowMode.Normal; //PowMode��Normal�ɖ߂�
        }
    }

}
