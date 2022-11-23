using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonovihair<GameManager>
{
    [Header("Pow�u���b�N�̐��̐ݒ�")]
    [Tooltip("Pow�u���b�N�̍ő�̐�")]
    [SerializeField]int _maxPow = 100;
    [Tooltip("�Q�[�����ɓ���Pow�u���b�N�̐�")]
    private int _pow;
    [Tooltip("Pow�̐����J�E���g����e�L�X�g")]
    private Text _powCount;
    [Tooltip("GameOver�ɂȂ����ۃL�����o�X�̕\��")]
    private GameObject _gameOverCanvas;
    [Tooltip("�`�F�b�N�|�C���g���Q�Ƃ���ϐ�")]
    GameObject[] _chackObj;
    [Tooltip("�`�F�b�N�|�C���g���X�g")]
    List<GameObject> _chackList = new List<GameObject>();
    [Header("�q�G�����L�[�̏ォ�珇�ԂɃ`�F�b�N�|�C���g���J�E���g(0�X�^�[�g)")]
    [Tooltip("�`�F�b�N�|�C���g�̃J�E���g")]
    public int _chackCount = 0;
    [Tooltip("Player��Transform")]
    Transform _playerTrans;
    [Tooltip("Score�̑���")]
    GameObject _plasMinas;
    [Tooltip("�Q�[����")]
    public bool _playGame;
    //�O����ϐ��̒��g��ς��邱�Ƃ��Ȃ������牺�̕��ɕύX���܂��B
    //public bool _playGame => _play;
    [Tooltip("�`�F�b�N�|�C���g�ʉߎ��̃G�t�F�N�g")]
    [SerializeField] GameObject _collect;

    //�V�[�������p���ł������Ȃ����I
    protected override bool _dontDestroyOnLoad { get { return true; } }

    /// <summary>��ԏ��߂ɂ���Ăق������Ƃ͂����ɏ����B</summary>
    void Start()
    {
        _gameOverCanvas = transform.GetChild(0).gameObject;
        _gameOverCanvas.SetActive(false);
        //pow�u���b�N�̏�����
        PowReset();
        Begin();
    }

    /// <summary>���[�h����x�A���߂ɂ���Ăق������Ƃ̃��\�b�h�B</summary>
    void Begin()
    {
        //pow�̎c������J�E���g����e�L�X�g�̎Q��
        _powCount = GameObject.Find("Count")?.GetComponent<Text>();
        //�e�L�X�g�̕\��
        ShowText(_powCount);
        //�X�^�[�g�n�_�ƃ`�F�b�N�|�C���g��S���Q��
        _chackObj = GameObject.FindGameObjectsWithTag("Respawn");
        //_chackObj��null����
        if (_chackObj.Length != 0)
        {
            //�Q�Ƃ����X�^�[�g�n�_�ƃ`�F�b�N�|�C���g�����X�g�ɒǉ�
            foreach (var i in _chackObj)
            {
                _chackList.Add(i);
            }
        }
        //player��Transform���Q��
        _playerTrans = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();
        _plasMinas = GameObject.Find("minas");
        if (_plasMinas) _plasMinas.SetActive(false);
        _playGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        //R�L�[��������player��y�̃|�W�V������-10�����Ȃ�
        if(Input.GetKeyDown(KeyCode.R) || _playerTrans.position.y < -10f)
        {
            //Pow�̌������炵�ă`�F�b�N�|�C���g�Ƀ��[�v������
            AddPow(-5);
            ChackPointWarp();
        }
    }

    /// <summary>Pow�u���b�N�̎c����̕\��</summary>
    public void ShowText(Text powcount)
    {
        //null����
        if (powcount)
        {
            //�e�L�X�g�Ɏc����̕\��
            powcount.text = _pow.ToString("000");
        }
    }

    /// <summary>Pow�u���b�N�̐��𑝌�������B</summary>
    /// <param name="add">����������l</param>
    public void AddPow(int add)
    {
        //�u���b�N�̐��𑝌������Ă���e�L�X�g�ɕ\���B
        _pow += add;
        if (_plasMinas) _plasMinas.SetActive(true);
        var minastext = _plasMinas?.GetComponent<Text>();
        minastext.text = add.ToString("+#;-#;");
        ShowText(_powCount);
        if(_pow <= 0)
        {
            _playGame = false;
            _gameOverCanvas.SetActive(true);
        }
    }

    /// <summary>�c����Pow�u���b�N�̐��ɂ���ĕ]����\������</summary>
    /// <returns>Rank��Ԃ�</returns>
    public string Rank()
    {
        //50�ȏ��RankS�A30�ȏ��RankA�A10�ȏ��RankB�A����ȉ���RankC
        if(_pow >= 50)
        {
            return "Rank S";
        }
        else if(_pow >= 30)
        {
            return "Rank A";
        }
        else if(_pow >= 10)
        {
            return "Rank B";
        }
        else
        {
            return "Rank C";
        }
    }

    /// <summary>Player��Chackpoint�Ƀ��[�v������</summary>
    public void ChackPointWarp()
    {
        if(_playerTrans && _chackList.Count != 0)
        _playerTrans.position = _chackList[_chackCount].transform.position;
    }

    public void ChackpointGet()
    {
        var _chackAni = _chackList[_chackCount]?.GetComponent<Animator>();
        _chackAni.Play("ChackTrueAni");
        Instantiate(_collect, _chackList[_chackCount].transform.position,Quaternion.identity);
    }

    /// <summary>Pow�u���b�N�̏�����������</summary>
    public void PowReset()
    {
        _pow = _maxPow;
    }

    private void OnLevelWasLoaded(int level)
    {
        _playGame = false;
        //Chackpoint�̃��X�g��S������������B
        _chackList.Clear();
        _chackCount = 0;
        //���[�h����x���\�b�h���Ăяo���B
        Begin();
    }
}
