using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] bool _enemytipe = true;
    Rigidbody2D _rb;
    [SerializeField] float _movespeed; //�ړ����x
    [SerializeField] Vector2 _linegraund = new Vector2(1f, -1f); //���̌��o
    [SerializeField] Vector2 _groundwalk = Vector2.right; //�L�����̌���
    [SerializeField] Vector2 _wallground = new Vector2(1f, 0f); //�ǂ̌��o
    [SerializeField] LayerMask _groundLayer;�@//����Layer
    [SerializeField] LayerMask _wallLayer; //�ǂ�Layer
    bool _groundbool = false; //���̔���
    bool _wallbool = false;�@//�ǂ̔���

    Transform _player; //Player��Position
    [SerializeField]float _cooltime; //������܂ł̃C���^�[�o��
    float _time;�@//���Ԑ}��
    [SerializeField] float _fallmovespeed;�@//�����̓���
    [SerializeField] float _downspeed;�@//�����鑬�x
    [SerializeField] Vector2 _fallground = new Vector2(0f, -10f);�@//LineCast�̒���
    bool _boolground = true;�@//�n�ʂɂ����ۂ̔���
    float _nowtransform;�@//�ǂ����񂪏��߂ɂ���Y����Position�̋L�^

    [SerializeField] float _throwcooltime; //���܂ł�cooltime
    [SerializeField] GameObject _bullet; //�e
    [SerializeField] GameObject _shotpoint;�@//���ꏊ
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
        if(_enemytipe == true)�@//�̂��̂���
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
                Debug.Log("���o����ĂȂ�");
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
        else //�̂��̂���
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
    void FallZako() //�ǂ�����
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
            Debug.Log("Player�̈ʒu�����o���܂���");
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
