using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D _playerRb = default;
    SpriteRenderer _playerSr = default;
    Animator _playerAnim = default;
    [SerializeField, Tooltip("移動スピード")] float _speed;
    [Tooltip("左右入力")] float _hInput;
    [SerializeField, Tooltip("ジャンプ力")] float _jumpForce;
    [SerializeField, Tooltip("着地判定")] bool _onGround;


    // Start is called before the first frame update
    void Start()
    {
        _playerRb = gameObject.GetComponent<Rigidbody2D>();
        _playerSr = gameObject.GetComponent<SpriteRenderer>();
        _playerAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
    }

    void LateUpdate()
    {

        if (_playerAnim)
        {
            _playerAnim.SetFloat("Speed", Mathf.Abs(_hInput * _speed));

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _playerAnim.SetTrigger("Pow");
            }

            _playerRb.velocity = new Vector2(_hInput * _speed, _playerRb.velocity.y);


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
            }


        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
    }
}
