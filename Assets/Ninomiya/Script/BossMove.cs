using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMove : MonoBehaviour
{
    Rigidbody2D _rb;
    Vector3 _playerPosition;
    Vector3 _bossPosition;
    [SerializeField]float _hp;�@//�{�X��HP
    float _time; 
    [SerializeField] float _cooltime; //�U���̃C���^�[�o��
    [SerializeField] GameObject _shotpoint;�@//�e�𔭎˂���ʒu
    [SerializeField] GameObject _buleet; //���˂���e
    [SerializeField] float _moveSpeed;�@//�ړ����x
    float _distance;�@//Player�ƃ{�X�̋���
    [SerializeField] float _stopdistance; //�ːi����ۂ̒�~����ʒu�ڈ�
    [SerializeField] Transform[] _targettransform; //�U�R�G�𐶐�����ꏊ
    [SerializeField] float _enemyCount;�@//�U�R�G�𐶐������
    Animator _anim;
    [SerializeField] GameObject[] _zako;
    [SerializeField] Text _hpText;
    AudioSource _audio;
    [SerializeField] AudioClip _sound;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _audio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BossAttack();
        if(_hp <= 0)
        {
            _anim.SetTrigger("Bossdown");
            Destroy(gameObject,0.5f);
            _audio.PlayOneShot(_sound);
        }
        _hpText.text = $"HP:{_hp}";
    }
    void BossAttack()�@//�{�X�̍U�����@
    {
        _bossPosition = transform.position;
        _playerPosition = GameObject.Find("Player").transform.position;
        _distance = Vector2.Distance(_bossPosition, _playerPosition);
        Debug.Log(_distance);
        _time += Time.deltaTime;
        if(_time > _cooltime)
        {
            int random = Random.Range(1, 10);
            _anim.SetBool("BossMove", false);
            if (random == 1 || random == 2 || random == 3) //�ːi�U��
            {
                var npvec = _playerPosition;
                var mvec = npvec - this.transform.position;
                _rb.velocity = mvec.normalized * _moveSpeed;
                _anim.SetBool("BossMove", true);
                if(_distance < _stopdistance)
                {
                    _anim.SetBool("BossMove", false);
                    _rb.velocity = Vector2.zero;
                    _time = 0;
                }
            }
            else if (random == 4 || random == 5 || random == 6)�@//�e�̔���
            {
                _anim.SetBool("BossAttack1", true);
                Instantiate(_buleet, _shotpoint.transform.position, transform.rotation);
                _time = 0;
                _anim.SetBool("BossAttack1", false);
            }
            else //�U�R�G�̐���
            {
                for(int i = 0; i < _enemyCount; i++)
                {
                    var InstRandam = Random.Range(1, 4);
                    Instantiate(_zako[InstRandam], _targettransform[InstRandam].position, transform.rotation);
                    _anim.SetBool("BossAttack1", true);
                }
                _time = 0;
                _anim.SetBool("BossAttack1", false);
            }
        }
        if(_bossPosition.x < _playerPosition.x) //�{�X�̌���
        {
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else
        {
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }
    public void BossHp(float damage) //�{�X�̗̑�
    {
        _hp -= damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)�@//�̗͂̌���
    {
        if(collision.gameObject.tag == "Pow")
        {
            BossHp(1);
        }
    }
}
