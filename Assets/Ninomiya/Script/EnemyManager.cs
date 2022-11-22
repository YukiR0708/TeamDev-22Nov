using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] bool _enemytipe = true;
    Rigidbody2D _rb;
    [SerializeField] float _movespeed; //移動速度
    [SerializeField] Vector2 _linegraund = new Vector2(1f, -1f); //床の検出
    [SerializeField] Vector2 _groundwalk = Vector2.right; //キャラの向き
    [SerializeField] Vector2 _wallground = new Vector2(1f, 0f); //壁の検出
    [SerializeField] LayerMask _groundLayer;　//床のLayer
    [SerializeField] LayerMask _wallLayer; //壁のLayer
    bool _groundbool = false; //床の判定
    bool _wallbool = false;　//壁の判定

    Transform _player; //PlayerのPosition
    [SerializeField]float _cooltime; //落ちるまでのインターバル
    float _time;　//時間図る
    [SerializeField] float _fallmovespeed;　//横軸の動き
    [SerializeField] float _downspeed;　//落ちる速度
    [SerializeField] Vector2 _fallground = new Vector2(0f, -10f);　//LineCastの長さ
    bool _boolground = true;　//地面についた際の判定
    float _nowtransform;　//どっすんが初めにいたY軸のPositionの記録

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
        //Zako();
        //FallZako();
        ThrowZako();
    }
    void Zako()
    {
        if(_enemytipe == true)　//のこのこ赤
        {
            Vector2 start = this.transform.position;
            Debug.DrawLine(start, start + _linegraund);
            Debug.DrawLine(start, start + _wallground);
            RaycastHit2D hit = Physics2D.Linecast(start, start + _linegraund, _groundLayer);
            RaycastHit2D hit2 = Physics2D.Linecast(start, start + _wallground, _wallLayer);
            if (!hit2.collider && hit.collider)
            {
                _rb.velocity = _groundwalk * _movespeed;
            }
            else
            {
                Debug.Log("検出されてない");
                if (_groundbool == true)
                {
                    _groundwalk = Vector2.right;
                    this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    _linegraund = new Vector2(1f, -1f);
                    _wallground = new Vector2(1f, 0f);
                    _groundbool = false;
                }
                else
                {
                    _groundwalk = Vector2.left;
                    this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    _linegraund = new Vector2(-1f, -1f);
                    _wallground = new Vector2(-1f, 0f);
                    _groundbool = true;
                }
            }
        }
        else //のこのこ緑
        {
            Vector2 start = this.transform.position;
            Debug.DrawLine(start, start + _wallground);
            RaycastHit2D hit2 = Physics2D.Linecast(start, start + _wallground, _wallLayer);
            if(!hit2.collider)
            {
                _rb.velocity = _groundwalk * _movespeed;
            }
            else
            {
                if(_wallbool == true)
                {
                    _groundwalk = Vector2.right;
                    this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    _wallground = new Vector2(1f, 0f);
                    _wallbool = false;
                }
                else
                {
                    _groundwalk = Vector2.left;
                    this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    _wallground = new Vector2(-1f, 0f);
                    _wallbool = true;
                }
            }
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
            Vector3 dir = (new Vector3(hit.point.x, hit.point.y, 0) - this.transform.position);
            _rb.velocity = dir.normalized * _fallmovespeed;
            if (_boolground == false)
            {
                _rb.velocity = new Vector3(0f, _nowtransform,0f) * _downspeed;
                if (_nowtransform < this.gameObject.transform.position.y)
                {
                    _time = 0;
                    _boolground = true;
                    _rb.velocity = Vector3.zero;
                }
            }
        }
        else if(_time < _cooltime && _player)
        {
            Vector3 dir2 = (_player.position - this.transform.position).normalized * _fallmovespeed;
            _rb.velocity = dir2.normalized * _fallmovespeed;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
    void ThrowZako()
    {
        if(_player)
        {
            _time += Time.deltaTime;
            if(_time > _throwcooltime)
            {
                Instantiate(_bullet, _shotpoint.transform.position, transform.rotation);
                _time = 0;
            }
            if(_player.transform.position.x < this.transform.position.x)
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
        if(collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject.transform;
            Debug.Log("Playerの位置を検出しました");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _boolground = false;
        }
    }
}
