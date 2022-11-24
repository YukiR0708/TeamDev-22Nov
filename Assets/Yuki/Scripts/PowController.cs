using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowController : MonoBehaviour
{
    PlayerController _playerCon = default;
    SpriteRenderer _playerSR = default;
    Rigidbody2D _powRb = default;
    Animation _anim = default;
    BoxCollider2D _powCol = default;
    AudioSource _powAS;
    [SerializeField, Header("Pow���Ȃɂ��ɓ�������SE")] AudioClip _powSE;
    [SerializeField, Header("Pow�𓊂����Ƃ��ɏo��g��")] GameObject _shockWave = default;
    [SerializeField, Header("�G��|�����Ƃ��ɏo��G�t�F�N�g")] GameObject _kirakira = default;
    [SerializeField, Header("Pow���������ԍ���")] float _destroyTimeOffset = default;
    [SerializeField, Header("�U�R�G���U������܂ł̎��ԍ���")] float _attackTimeOffset = default;
    [SerializeField, Header("Pow�𓊂���p�x")] float _theta = default;
    [SerializeField, Header("Pow�𓊂��鏉���x")] float _initialV = default;

    /// <summary> �ePow�̖�������  /// </summary>
    private enum PowJudge
    {
        Thrown, //������ꂽPow
        Put�@//�u���ꂽPow
    }

    private PowJudge _powJudge;

    private void Awake()
    {
        _playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerSR = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        _powRb = gameObject.GetComponent<Rigidbody2D>();
        _anim = gameObject.GetComponent<Animation>();
        _powCol = gameObject.GetComponent<BoxCollider2D>();
        _powAS = gameObject.GetComponent<AudioSource>();

        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //�������[�h��������
        {
            gameObject.layer = LayerMask.NameToLayer("ThrownPow");  //Player�ƂԂ���Ȃ��悤�A�����̃��C���[��ς���
            _powJudge = PowJudge.Thrown;
        }
        else if (_playerCon._playMode == PlayerController.PowMode.Put)
        {
            gameObject.layer = LayerMask.NameToLayer("PutPow");  //�G���܂�Ԃ��悤�A�����̃��C���[��ς���
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

        _playerCon._playMode = PlayerController.PowMode.Normal; //PowMode��Normal�ɖ߂�


    }

    /// <summary> ������ꂽ�Ƃ��̏��� /// </summary>
    void Thrown()
    {
        _powRb.bodyType = RigidbodyType2D.Dynamic;  //�����̂Ƃ��͕�������������

        _theta = _playerSR.flipX ? Mathf.PI - _theta : _theta;//Player�̃X�v���C�g���ǂ����������Ă邩�ŃV�[�^�𔽓]������
        _powRb.velocity = new Vector2(_initialV * Mathf.Cos(_theta), _initialV * Mathf.Sin(_theta)); //�Ε�����

    }

    /// <summary> �u���ꂽ�Ƃ��̏��� </summary>
    void Put()
    {
        Debug.Log("�u�������Ƃ̏���");
        _powRb.bodyType = RigidbodyType2D.Static;   //�u���Ƃ��͌Œ肷��
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name + "�ɓ��������I");
        if (_powJudge == PowJudge.Thrown && !other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PowDestroyCoroutine());

            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow") || other.gameObject.CompareTag("Wall")) //�������[�h�Œn�ʂ��ǂɂ���������
            {
                Debug.Log("�U�R��������");
                _powAS.PlayOneShot(_powSE);
                //SE�炵�ĉ�ʂ�U��������B�g���o���B
                StartCoroutine(DestroyZakoCoroutine());



            }
            else if (other.gameObject.CompareTag("ShakeDown") || other.gameObject.CompareTag("HitDown")) //�U�R�����G�ɂ���������
            {
                Debug.Log("���������G�ƃU�R������");
                _powAS.PlayOneShot(_powSE);
                //SE�炵�ĉ�ʂ�U��������B�g�������B
                Destroy(other.gameObject); //���������G�Ɖ�ʓ��̃U�R�G���|���
                StartCoroutine(DestroyZakoCoroutine());

            }

            else if (other.gameObject.CompareTag("Boss"))
            {
                Debug.Log("Boss��HP������");
                //Boss��HP������
            }

        }

    }

    private IEnumerator PowDestroyCoroutine()
    {
        _powRb.bodyType = RigidbodyType2D.Static;   //Pow�̓������~�߂�
        _powCol.enabled = false;
        Instantiate(_shockWave, transform.position, transform.rotation);
        _anim.Play();//Pow�̃A�j���[�V�����Đ�
        yield return new WaitForSeconds(_destroyTimeOffset);    //�w��b�҂�
        Destroy(this.gameObject);
    }

    /// <summary> �U�R�G�������擾���ď����B��ʓ��O�̃^�O����͓G���̃X�N���v�g�ɋL�q </summary>
    private IEnumerator DestroyZakoCoroutine()
    {
        GameObject[] zakos = GameObject.FindGameObjectsWithTag("ShakeDown");

        foreach(GameObject enemies in zakos)
        {
            Instantiate(_kirakira, enemies.transform);
        }

        yield return new WaitForSeconds(_attackTimeOffset);
        foreach (GameObject enemies in zakos)
        {
            Destroy(enemies);
        }

    }
}

