using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ///*****�ړ����W�����v�֘A*****
    Rigidbody2D _playerRb = default;
    SpriteRenderer _playerSr = default;
    Animator _playerAnim = default;
    [SerializeField, Header("�ړ��X�s�[�h")] float _speed;
    [Tooltip("���E����")] float _hInput;
    [SerializeField, Header("�W�����v��")] float _jumpForce;
    [SerializeField, Header("���n����")] bool _onGround;

    //*****Pow�u���b�N�֘A*****
    [Tooltip("Pow�𓊂����邩�ǂ���")] bool _canThrow;
    [SerializeField, Header("Pow�𓊂���C���^�[�o��")] float _throwInterval;
    [SerializeField, Header("Pow�u���b�N")] GameObject _powBlock;
    [SerializeField, Header("������Pow�u���b�N����������ʒu")] GameObject _powSpawn;


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
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        //�J�[�\���̈ʒu���擾
    }

    void LateUpdate()
    {

        if (_playerAnim && _playMode == PowMode.Normal)
        {
            _playerAnim.SetFloat("Speed", Mathf.Abs(_hInput * _speed)); //AnimatorController�Ɉړ����x��n��
            //�J�[�\���̈ʒu�Ƀ}�[�J�[������
            
            //*****Pow�u���b�N�𓊂��鏈���i�C���^�[�o������j*****
            if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow)
            {
                _playerAnim.SetTrigger("Pow");  //������i�A�j���[�V�����j
                Instantiate(_powBlock, _powSpawn.transform);
                _playMode = PowMode.Throw; //������iPowController����Q�Ɓj
                //Pow�̐������炷
            }

            //*****Pow�u���b�N��z�u���鏈��*****
            if (Input.GetButtonDown("Fire1"))
            {
                //�J�[�\���̈ʒu��Pow�u���b�N��z�u����
                _playMode = PowMode.Put; //�z�u����iPowController����Q�Ɓj
                //Pow�̐������炷
            }

            //*****�ړ����W�����v����*****
            _playerRb.velocity = new Vector2(_hInput * _speed, _playerRb.velocity.y);

            //�ړ����̔��]
            if (_hInput < 0 && !_playerSr.flipX)
            {
                _playerSr.flipX = true;
                _powSpawn.transform.position = new Vector2(-_powSpawn.transform.position.x, _powSpawn.transform.position.y);
            }
            else if (_hInput > 0 && _playerSr.flipX)
            {
                _playerSr.flipX = false;
                _powSpawn.transform.position = new Vector2(-_powSpawn.transform.position.x, _powSpawn.transform.position.y);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _onGround)
            {
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _playerAnim.SetTrigger("Jump");
            }
        }
    }
    /// <summary> ���n����̃t���O���� /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
        //Elevator��������q�I�u�W�F�N�g�ɂ���
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
        //Elevator��������q�I�u�W�F�N�g�����������
    }
}
