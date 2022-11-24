using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
    [SerializeField] bool _enemyType = true;
    Rigidbody2D _rb;
    Transform _player; //PlayerのPosition
    [SerializeField] float _cooltime; //落ちるまでのインターバル
    float _time;　//時間図る
    [SerializeField] float _fallmovespeed;　//横軸の動き
    [SerializeField] float _downspeed;　//落ちる速度
    [SerializeField] Vector2 _fallground = new Vector2(0f, -10f);　//LineCastの長さ
    bool _boolground = true;　//地面についた際の判定
    float _nowtransform; //どっすんが初めにいたY軸のPositionの記録
    [SerializeField] LayerMask _groundLayer;　//床のLayer

    [SerializeField] float _throwcooltime; //撃つまでのcooltime
    [SerializeField] GameObject _bullet; //弾
    [SerializeField] GameObject _shotpoint;　//撃つ場所
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _nowtransform = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemyType == true)
        {
            FallZako();
        }
        else
        {
            ThrowZako();
        }
    }
    void FallZako() //どっすん
    {
        _time += Time.deltaTime;
        Vector2 start = this.transform.position;
        Debug.DrawLine(start, start + _fallground);
        RaycastHit2D hit = Physics2D.Linecast(start, start + _fallground, _groundLayer);
        if (_time > _cooltime)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector3 dir = (new Vector3(hit.point.x, hit.point.y, 0) - this.transform.position);
            _rb.velocity = dir.normalized * _fallmovespeed;
            if (_boolground == false)
            {
                _rb.velocity = new Vector3(0f, _nowtransform, 0f) * _downspeed;
                if (_nowtransform < this.gameObject.transform.position.y)
                {
                    _time = 0;
                    _boolground = true;
                    _rb.velocity = Vector3.zero;
                }
            }
        }
        else if (_time < _cooltime && _player)
        {
            Vector3 dir2 = (_player.position - this.transform.position).normalized * _fallmovespeed;
            _rb.velocity = dir2.normalized * _fallmovespeed;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
    void ThrowZako() //射程に入ると撃ってくる敵
    {
        if (_player)
        {
            _time += Time.deltaTime;
            if (_time > _throwcooltime)
            {
                Instantiate(_bullet, _shotpoint.transform.position, transform.rotation);
                _time = 0;
            }
            if (_player.transform.position.x < this.transform.position.x)
            {
                this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            else
            {
                this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject.transform;
            Debug.Log("Playerの位置を検出しました");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _boolground = false;
        }
    }
}
