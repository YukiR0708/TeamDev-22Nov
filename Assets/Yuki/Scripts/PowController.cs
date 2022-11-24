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
    [SerializeField, Header("Powがなにかに当たったSE")] AudioClip _powSE;
    [SerializeField, Header("Powを投げたときに出る波動")] GameObject _shockWave = default;
    [SerializeField, Header("敵を倒したときに出るエフェクト")] GameObject _kirakira = default;
    [SerializeField, Header("Powを消す時間差分")] float _destroyTimeOffset = default;
    [SerializeField, Header("ザコ敵を攻撃するまでの時間差分")] float _attackTimeOffset = default;
    [SerializeField, Header("Powを投げる角度")] float _theta = default;
    [SerializeField, Header("Powを投げる初速度")] float _initialV = default;

    /// <summary> 各Powの役割識別  /// </summary>
    private enum PowJudge
    {
        Thrown, //投げられたPow
        Put　//置かれたPow
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

        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //投げモードだったら
        {
            gameObject.layer = LayerMask.NameToLayer("ThrownPow");  //Playerとぶつからないよう、自分のレイヤーを変える
            _powJudge = PowJudge.Thrown;
        }
        else if (_playerCon._playMode == PlayerController.PowMode.Put)
        {
            gameObject.layer = LayerMask.NameToLayer("PutPow");  //敵が折り返すよう、自分のレイヤーを変える
            _powJudge = PowJudge.Put;
        }

    }
    void Start()
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //投げモードだったら
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
        _powRb.bodyType = RigidbodyType2D.Dynamic;  //投げのときは物理挙動させる

        _theta = _playerSR.flipX ? Mathf.PI - _theta : _theta;//Playerのスプライトがどっちを向いてるかでシータを反転させる
        _powRb.velocity = new Vector2(_initialV * Mathf.Cos(_theta), _initialV * Mathf.Sin(_theta)); //斜方投射

    }

    /// <summary> 置かれたときの処理 </summary>
    void Put()
    {
        Debug.Log("置いたあとの処理");
        _powRb.bodyType = RigidbodyType2D.Static;   //置くときは固定する
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name + "に当たった！");
        if (_powJudge == PowJudge.Thrown && !other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PowDestroyCoroutine());

            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow") || other.gameObject.CompareTag("Wall")) //投げモードで地面か壁にあたったら
            {
                Debug.Log("ザコだけしぬ");
                _powAS.PlayOneShot(_powSE);
                //SE鳴らして画面を振動させる。波を出す。
                StartCoroutine(DestroyZakoCoroutine());



            }
            else if (other.gameObject.CompareTag("ShakeDown") || other.gameObject.CompareTag("HitDown")) //ザコか強敵にあたったら
            {
                Debug.Log("当たった敵とザコがしぬ");
                _powAS.PlayOneShot(_powSE);
                //SE鳴らして画面を振動させる。波をだす。
                Destroy(other.gameObject); //当たった敵と画面内のザコ敵が倒れる
                StartCoroutine(DestroyZakoCoroutine());

            }

            else if (other.gameObject.CompareTag("Boss"))
            {
                Debug.Log("BossのHPが削れる");
                //BossのHPけずる
            }

        }

    }

    private IEnumerator PowDestroyCoroutine()
    {
        _powRb.bodyType = RigidbodyType2D.Static;   //Powの動きを止める
        _powCol.enabled = false;
        Instantiate(_shockWave, transform.position, transform.rotation);
        _anim.Play();//Powのアニメーション再生
        yield return new WaitForSeconds(_destroyTimeOffset);    //指定秒待つ
        Destroy(this.gameObject);
    }

    /// <summary> ザコ敵だけを取得して消す。画面内外のタグ制御は敵側のスクリプトに記述 </summary>
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

