using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowController : MonoBehaviour
{
    PlayerController _playerCon;
    Rigidbody2D _powRb = default;
   [SerializeField, Header("Pow�𓊂���C���^�[�o��")] float _throwInterval = default;

    /// <summary> �ePow�̖�������  /// </summary>
    private enum PowJudge
    {
        Thrown, //������ꂽPow
        Put�@//�u���ꂽPow
    }

    private PowJudge _powJudge;

    private void Awake()
    {
        _playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        _powRb = gameObject.GetComponent<Rigidbody2D>();

        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //�������[�h��������
        {
            gameObject.layer = LayerMask.NameToLayer("ThrownPow");  //Player�ƂԂ���Ȃ��悤�A�����̃��C���[��ς���
            _powJudge = PowJudge.Thrown;
        }
        else if(_playerCon._playMode == PlayerController.PowMode.Put)
        {
            _powJudge = PowJudge.Put;
        }

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw && _powJudge == PowJudge.Thrown)
        {
            StartCoroutine(ThrowCoroutine());

            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow")) //�������[�h�Œn�ʂ��ǂɂ���������
            {
                Debug.Log("�U�R��������");
                //SE�炵�ĉ�ʂ�U��������B�g���o���B
                DestroyZako();



            }
            else if (other.gameObject.CompareTag("ShakeDown") || other.gameObject.CompareTag("HitDown")) //�U�R�����G�ɂ���������
            {
                Debug.Log("���������G�ƃU�R������");
                //SE�炵�ĉ�ʂ�U��������B�g�������B
                Destroy(other.gameObject); //���������G�Ɖ�ʓ��̃U�R�G���|���
                DestroyZako();

            }

            else if(other.gameObject.CompareTag("Boss"))
            {
                Debug.Log("Boss��HP������");
            }

        }

    }

    private IEnumerator ThrowCoroutine()
    {
        yield return new WaitForSeconds(_throwInterval);    //�w��b�҂�
        _playerCon._playMode = PlayerController.PowMode.Normal; //PowMode��Normal�ɖ߂�
        Destroy(this.gameObject);
    }

    /// <summary> �U�R�G�������擾���ď����B��ʓ��O�̃^�O����͓G���̃X�N���v�g�ɋL�q </summary>
    void DestroyZako()
    {
        GameObject[] zakos = GameObject.FindGameObjectsWithTag("ShakeDown"); //
        foreach (GameObject enemies in zakos)
        {
            Destroy(enemies);
        }

    }
}
