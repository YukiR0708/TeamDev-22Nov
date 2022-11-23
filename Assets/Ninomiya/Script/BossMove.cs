using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    Rigidbody2D _rb;
    Vector3 _playerPosition;
    Vector3 _bossPosition;
    [SerializeField]float _hp;　//ボスのHP
    float _time; 
    [SerializeField] float _cooltime; //攻撃のインターバル
    [SerializeField] GameObject _shotpoint;　//弾を発射する位置
    [SerializeField] GameObject _buleet; //発射する弾
    [SerializeField] float _moveSpeed;　//移動速度
    float _distance;　//Playerとボスの距離
    [SerializeField] float _stopdistance; //突進する際の停止する位置目安
    [SerializeField] Transform[] _targettransform; //ザコ敵を生成する場所
    [SerializeField] float _enemyCount;　//ザコ敵を生成する回数
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BossAttack();
        if(_hp <= 0)
        {
            _anim.SetTrigger("Bossdown");
            Destroy(gameObject,0.5f);
        }
    }
    void BossAttack()　//ボスの攻撃方法
    {
        _bossPosition = transform.position;
        _playerPosition = GameObject.Find("Player").transform.position;
        _distance = Vector2.Distance(_bossPosition, _playerPosition);
        Debug.Log(_distance);
        _time += Time.deltaTime;
        if(_time > _cooltime)
        {
            int random = Random.Range(1, 10);
            if(random == 1 || random == 2 || random == 3) //突進攻撃
            {
                var npvec = _playerPosition;
                var mvec = npvec - this.transform.position;
                _rb.velocity = mvec.normalized * _moveSpeed;
                _anim.SetBool("BossMove", true);
                if(_distance < _stopdistance)
                {
                    _rb.velocity = Vector2.zero;
                    _anim.SetBool("BossMove", false);
                    _time = 0;
                }
            }
            else if (random == 4 || random == 5 || random == 6)　//弾の発射
            {
                _anim.SetTrigger("BossAttack");
                Instantiate(_buleet, _shotpoint.transform.position, transform.rotation);
                _time = 0;
            }
            else //ザコ敵の生成
            {
                for(int i = 0; i < _enemyCount; i++)
                {
                    var InstRandam = Random.Range(1, 4);
                    Instantiate(_buleet, _targettransform[InstRandam].position, transform.rotation);
                    _anim.SetTrigger("BossAttack");
                }
                _time = 0;
            }
        }
        if(_bossPosition.x < _playerPosition.x) //ボスの向き
        {
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else
        {
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }
    void BossHp(float damage) //ボスの体力
    {
        _hp -= damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)　//体力の減少
    {
        if(collision.gameObject.tag == "Pow")
        {
            BossHp(1);
        }
    }
}
