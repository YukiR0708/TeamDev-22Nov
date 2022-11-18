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
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Zako();
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
                    _linegraund = new Vector2(1f, -1f);
                    _wallground = new Vector2(1f, 0f);
                    _groundbool = false;
                }
                else
                {
                    _groundwalk = Vector2.left;
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
                    _wallground = new Vector2(1f, 0f);
                    _wallbool = false;
                }
                else
                {
                    _groundwalk = Vector2.left;
                    _wallground = new Vector2(-1f, 0f);
                    _wallbool = true;
                }
            }
        }
        
    }
}
