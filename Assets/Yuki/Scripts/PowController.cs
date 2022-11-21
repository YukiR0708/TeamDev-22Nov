using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowController : MonoBehaviour
{
    PlayerController _playerCon;
    Rigidbody2D _powRb = default;
   [SerializeField, Header("Powを投げるインターバル")] float _throwInterval = default;

    /// <summary> 各Powの役割識別  /// </summary>
    private enum PowJudge
    {
        Thrown, //投げられたPow
        Put　//置かれたPow
    }

    private PowJudge _powJudge;

    private void Awake()
    {
        _playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        _powRb = gameObject.GetComponent<Rigidbody2D>();

        if (_playerCon._playMode == PlayerController.PowMode.Throw)  //投げモードだったら
        {
            gameObject.layer = LayerMask.NameToLayer("ThrownPow");  //Playerとぶつからないよう、自分のレイヤーを変える
            _powJudge = PowJudge.Thrown;
        }
        else if(_playerCon._playMode == PlayerController.PowMode.Put)
        {
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

    }

    /// <summary> 投げられたときの処理 /// </summary>
    void Thrown()
    {
        _powRb.bodyType = RigidbodyType2D.Dynamic;  //投げのときは物理挙動させる
    }

    /// <summary> 置かれたときの処理 </summary>
    void Put()
    {
        Debug.Log("置いたあとの処理");
        _powRb.bodyType = RigidbodyType2D.Static;   //置くときは固定する
        _playerCon._playMode = PlayerController.PowMode.Normal; //PowModeをNormalに戻す
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_playerCon._playMode == PlayerController.PowMode.Throw && _powJudge == PowJudge.Thrown)
        {
            StartCoroutine(ThrowCoroutine());

            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pow")) //投げモードで地面か壁にあたったら
            {
                Debug.Log("ザコだけしぬ");
                //SE鳴らして画面を振動させる。波を出す。
                DestroyZako();



            }
            else if (other.gameObject.CompareTag("ShakeDown") || other.gameObject.CompareTag("HitDown")) //ザコか強敵にあたったら
            {
                Debug.Log("当たった敵とザコがしぬ");
                //SE鳴らして画面を振動させる。波をだす。
                Destroy(other.gameObject); //当たった敵と画面内のザコ敵が倒れる
                DestroyZako();

            }

            else if(other.gameObject.CompareTag("Boss"))
            {
                Debug.Log("BossのHPが削れる");
            }

        }

    }

    private IEnumerator ThrowCoroutine()
    {
        yield return new WaitForSeconds(_throwInterval);    //指定秒待つ
        _playerCon._playMode = PlayerController.PowMode.Normal; //PowModeをNormalに戻す
        Destroy(this.gameObject);
    }

    /// <summary> ザコ敵だけを取得して消す。画面内外のタグ制御は敵側のスクリプトに記述 </summary>
    void DestroyZako()
    {
        GameObject[] zakos = GameObject.FindGameObjectsWithTag("ShakeDown"); //
        foreach (GameObject enemies in zakos)
        {
            Destroy(enemies);
        }

    }
}

