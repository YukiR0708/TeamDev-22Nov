using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>トゲに当たった時にPowブロックを減らしてPlayerをワープさせるスクリプト</summary>
public class TrapTogeScripts : MonoBehaviour
{
    [SerializeField] AudioSource _trapAudio;

    private void Start()
    {
        _trapAudio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //トゲがプレイヤーに当たったら、Powブロックを減らして特定のチェックポイントへワープさせる。
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddPow(-5);
            _trapAudio.Play();
            GameManager.Instance.ChackPointWarp();
        }
    }
}
