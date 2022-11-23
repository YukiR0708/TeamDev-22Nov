using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    ///*****�ړ����W�����v�֘A*****
    Rigidbody2D _playerRb = default;
    SpriteRenderer _playerSr = default;
    Animator _playerAnim = default;
    [SerializeField, Header("�ړ��X�s�[�h")] float _speed = default;
    [Tooltip("���E����")] float _hInput = default;
    [SerializeField, Header("�W�����v��")] float _jumpForce = default;
    [SerializeField, Header("���n����")] bool _onGround = default;

    //*****Pow�u���b�N�֘A*****
    [Tooltip("�}�E�X�̃|�W�V����")] Vector2 _mouseposition = default;
    [Tooltip("Pow�𓊂����邩�ǂ���")] bool _canThrow = default;    //�C���^�[�o�������Đ���
    [Tooltip("Pow��u���邩�ǂ���")] bool _canPut = default;    //�J�[�\���̐�ɂ��̂����邩�ǂ����Ő���
    [SerializeField, Header("Pow�u���b�N")] GameObject _powBlock = default;
    [SerializeField, Header("������Pow�u���b�N����������ʒu")] GameObject _powSpawn = default;
    [SerializeField, Header("�g�̃I�u�W�F�N�g")] GameObject _cursor = default;
    [Tooltip("�g��SpriteRenderer")] SpriteRenderer _crsorSR = default;
    [Tooltip("Ray�̒���")] float _rayLength = default;
    [SerializeField, Header("Pow�𓊂���C���^�[�o��")] float _throwInterval = default;
    [Tooltip("�J�[�\���ƍ��W�̃Y��")] float _offset = -0.5f;


    /// <summary> �v���C���[�̑�����  /// </summary>
    public enum PowMode
    {
        Normal,  //Pow�𑀍삵�Ȃ���Ԃ̂Ƃ�
        Throw, //Pow�𓊂���Ƃ�
        Put�@//Pow��u���Ƃ�
    }

    public PowMode _playMode;


    void Start()
    {
        _playerRb = gameObject.GetComponent<Rigidbody2D>();
        _playerSr = gameObject.GetComponent<SpriteRenderer>();
        _playerAnim = gameObject.GetComponent<Animator>();
        Cursor.visible = false;
        _crsorSR = _cursor.GetComponent<SpriteRenderer>();

        _canPut = true;
        _canThrow = true;
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //�J�[�\���̈ʒu���擾
        _cursor.transform.position = _mouseposition;   //�g�I�u�W�F�N�g���J�[�\���̈ʒu�Ɉړ�

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //�J��������J�[�\���̕�����Ray���΂��B

        if(Physics2D.BoxCast(ray.origin, new Vector2(1.0f,1.0f),90, ray.direction, _rayLength) || !_onGround) //�����ɂ���������Pow��u���Ȃ��悤�ɂ���B�g�I�u�W�F�N�g��Ԃ�����B
        {
            Debug.Log("�z�u�ł��Ȃ���");
            _canPut = false;
            _crsorSR.color = Color.red;
        }
        else
        {
            _canPut = true;
            _crsorSR.color = Color.white;
        }

    }

    void LateUpdate()
    {

        if (_playerAnim && _playMode == PowMode.Normal)
        {
            _playerAnim.SetFloat("Speed", Mathf.Abs(_hInput * _speed)); //AnimatorController�Ɉړ����x��n��


            //*****Pow�u���b�N�𓊂��鏈��*****
            if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow)
            {
                _canThrow = false;
                StartCoroutine("ThrowCoroutine");
                _playerAnim.SetTrigger("Pow");  //������i�A�j���[�V�����j
                //�u�ۂ�[��vSE�炷
                _playMode = PowMode.Throw; //������iPowController����Q�Ɓj
                Instantiate(_powBlock, _powSpawn.transform.position, _powSpawn.transform.rotation);
                //Pow�̐������炷
            }

            //*****Pow�u���b�N��z�u���鏈��*****
            if (Input.GetButtonDown("Fire1") && _canPut)
            {
                _playMode = PowMode.Put; //�z�u����iPowController����Q�Ɓj
                Instantiate(_powBlock, _cursor.transform.position, _cursor.transform.rotation);�@//�g�̈ʒu��Pow�u���b�N��z�u����
                //Pow�̐������炷
            }
            else if (Input.GetButtonDown("Fire1") && !_canPut)
            {

                //�u�u�b�u�[�vSE�炷
            }

            //*****�ړ����W�����v����*****
            _playerRb.velocity = new Vector2(_hInput * _speed, _playerRb.velocity.y);

            //�ړ����̔��]
            if (_hInput < 0 && !_playerSr.flipX)
            {
                _playerSr.flipX = true;
            }
            else if (_hInput > 0 && _playerSr.flipX)
            {
                _playerSr.flipX = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && _onGround)
            {
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _playerAnim.SetTrigger("Jump");
                _onGround = false;
            }
        }
    }
    /// <summary> ���n����̃t���O���� �E�G���x�[�^�[�ł̕��������C�� /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow"))
        {
            _onGround = true;
        }
        else if(other.gameObject.CompareTag("Elevator"))
        {
            _onGround = true;
            transform.SetParent(other.gameObject.transform);    //Elevator��������q�I�u�W�F�N�g�ɂ���
        }
        
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Elevator"))
        {
            transform.SetParent(null);  //Elevator��������q�I�u�W�F�N�g����O��
        }
    }


    private IEnumerator ThrowCoroutine()
    {
        yield return new WaitForSeconds(_throwInterval);
        _canThrow = true;
    }
    
}
