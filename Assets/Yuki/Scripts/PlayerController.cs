using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ///*****移動＆ジャンプ関連*****
    Rigidbody2D _playerRb = default;
    SpriteRenderer _playerSr = default;
    Animator _playerAnim = default;
    [SerializeField, Header("移動スピード")] float _speed;
    [Tooltip("左右入力")] float _hInput;
    [SerializeField, Header("ジャンプ力")] float _jumpForce;
    [SerializeField, Header("着地判定")] bool _onGround;

    //*****Powブロック関連*****
    [Tooltip("Powを投げられるかどうか")] bool _canThrow;
    [SerializeField, Header("Powを投げるインターバル")] float _throwInterval;
    [SerializeField, Header("Powブロック")] GameObject _powBlock;
    [SerializeField, Header("投げるPowブロックを召喚する位置")] GameObject _powSpawn;


    /// <summary> プレイヤーの操作状態  /// </summary>
    public enum PowMode
    {
        Normal,  //Powを操作しない状態のとき
        Throw, //Powを投げるとき
        Put　//Powを置くとき
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
        //カーソルの位置を取得
    }

    void LateUpdate()
    {

        if (_playerAnim && _playMode == PowMode.Normal)
        {
            _playerAnim.SetFloat("Speed", Mathf.Abs(_hInput * _speed)); //AnimatorControllerに移動速度を渡す
            //カーソルの位置にマーカーをつける
            
            //*****Powブロックを投げる処理（インターバルあり）*****
            if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow)
            {
                _playerAnim.SetTrigger("Pow");  //投げる（アニメーション）
                Instantiate(_powBlock, _powSpawn.transform);
                _playMode = PowMode.Throw; //投げる（PowControllerから参照）
                //Powの数を減らす
            }

            //*****Powブロックを配置する処理*****
            if (Input.GetButtonDown("Fire1"))
            {
                //カーソルの位置にPowブロックを配置する
                _playMode = PowMode.Put; //配置する（PowControllerから参照）
                //Powの数を減らす
            }

            //*****移動＆ジャンプ処理*****
            _playerRb.velocity = new Vector2(_hInput * _speed, _playerRb.velocity.y);

            //移動時の反転
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
    /// <summary> 着地判定のフラグ処理 /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
        //Elevatorだったら子オブジェクトにする
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
        //Elevatorだったら子オブジェクトから解除する
    }
}
