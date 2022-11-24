using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    ///*****移動＆ジャンプ関連*****
    Rigidbody2D _playerRb = default;
    SpriteRenderer _playerSr = default;
    Animator _playerAnim = default;
    [SerializeField, Header("移動スピード")] float _speed = default;
    [Tooltip("左右入力")] float _hInput = default;
    [SerializeField, Header("ジャンプ力")] float _jumpForce = default;
    [SerializeField, Header("着地判定")] bool _onGround = default;

    //*****Powブロック関連*****
    [Tooltip("マウスのポジション")] Vector2 _mouseposition = default;
    [Tooltip("Powを投げられるかどうか")] bool _canThrow = default;    //インターバルをつけて制御
    [Tooltip("Powを置けるかどうか")] bool _canPut = default;    //カーソルの先にものがあるかどうかで制御
    [SerializeField, Header("Powブロック")] GameObject _powBlock = default;
    [SerializeField, Header("投げるPowブロックを召喚する位置")] GameObject _powSpawn = default;
    [SerializeField, Header("枠のオブジェクト")] GameObject _cursor = default;
    [Tooltip("枠のSpriteRenderer")] SpriteRenderer _crsorSR = default;
    [Tooltip("Rayの長さ")] float _rayLength = default;
    [SerializeField, Header("Powを投げるインターバル")] float _throwInterval = default;

    //*****SE関係*****
    AudioSource _audioSource = default;
    [SerializeField, Header("ジャンプの音")] AudioClip _jumpSE;
    [SerializeField, Header("Powを投げる音")] AudioClip _powThrowSE;
    [SerializeField, Header("Pow置けないときの音")] AudioClip _cannnotPutSE;
    [SerializeField, Header("Powを置く音")] AudioClip _putSE;
    [SerializeField, Header("敵に当たった音")] AudioClip _damageSE;

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
        _crsorSR = _cursor.GetComponent<SpriteRenderer>();
        _audioSource = gameObject.GetComponent<AudioSource>();

        _canPut = true;
        _canThrow = true;
    }

    private void Update()
    {
        if (GameManager.Instance._playGame)
        {
            _hInput = Input.GetAxisRaw("Horizontal");
            _mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //カーソルの位置を取得
            _cursor.transform.position = _mouseposition;   //枠オブジェクトをカーソルの位置に移動

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //カメラからカーソルの方向にRayを飛ばす。

            if (Physics2D.BoxCast(ray.origin, new Vector2(1.0f, 1.0f), 90, ray.direction, _rayLength) || !_onGround) //何かにあたったらPowを置けないようにする。枠オブジェクトを赤くする。
            {
                Debug.Log("配置できないよ");
                _canPut = false;
                _crsorSR.color = Color.red;
            }
            else
            {
                _canPut = true;
                _crsorSR.color = Color.white;
            }
        }
    }

    void LateUpdate()
    {

        if (GameManager.Instance._playGame)
        {
            if (_playerAnim && _playMode == PowMode.Normal)
            {
                _playerAnim.SetFloat("Speed", Mathf.Abs(_hInput * _speed)); //AnimatorControllerに移動速度を渡す


                //*****Powブロックを投げる処理*****
                if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow)
                {
                    _canThrow = false;
                    StartCoroutine("ThrowCoroutine");
                    _playerAnim.SetTrigger("Pow");  //投げる（アニメーション）
                    _audioSource.PlayOneShot(_powThrowSE); //「ぽよーん」SE鳴らす
                    _playMode = PowMode.Throw; //投げる（PowControllerから参照）
                    Instantiate(_powBlock, _powSpawn.transform.position, _powSpawn.transform.rotation);
                    GameManager.Instance.AddPow(-1); //Powの数を減らす
                }

                //*****Powブロックを配置する処理*****
                if (Input.GetButtonDown("Fire1") && _canPut)
                {
                    _playMode = PowMode.Put; //配置する（PowControllerから参照）
                    Instantiate(_powBlock, _cursor.transform.position, _cursor.transform.rotation); //枠の位置にPowブロックを配置する
                    _audioSource.PlayOneShot(_putSE); //「Powを置く」SE鳴らす
                    GameManager.Instance.AddPow(-1);  //Powの数を減らす
                }
                else if (Input.GetButtonDown("Fire1") && !_canPut)
                {

                    _audioSource.PlayOneShot(_cannnotPutSE); //「ブッブー」SE鳴らす
                }

                //*****移動＆ジャンプ処理*****
                _playerRb.velocity = new Vector2(_hInput * _speed, _playerRb.velocity.y);

                //移動時の反転
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
                    _audioSource.PlayOneShot(_jumpSE);
                    _playerAnim.SetTrigger("Jump");
                    _onGround = false;
                }
            }
        }
    }
    /// <summary> 着地判定のフラグ処理 ・エレベーターでの物理挙動修正 /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        string otherName = other.gameObject.tag;

        if (otherName == "Ground" || otherName == "Pow")
        {
            _onGround = true;
        }
        else if (otherName == "Elevator")
        {
            _onGround = true;
            transform.SetParent(other.gameObject.transform);    //Elevatorだったら子オブジェクトにする
        }
        else if (otherName == "ShakeDown" || otherName == "HitDown" || otherName == "Boss" || otherName == "Bullet" || otherName == "Ball")
        {
            _audioSource.PlayOneShot(_damageSE);
            GameManager.Instance.AddPow(-1); //敵に当たったらPowブロックの数が減る
        }

    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            transform.SetParent(null);  //Elevatorだったら子オブジェクトから外す
        }
    }


    private IEnumerator ThrowCoroutine()
    {
        yield return new WaitForSeconds(_throwInterval);
        _canThrow = true;
    }

}
