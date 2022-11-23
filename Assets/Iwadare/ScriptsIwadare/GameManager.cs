using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonovihair<GameManager>
{
    [Header("Powブロックの数の設定")]
    [Tooltip("Powブロックの最大の数")]
    [SerializeField]int _maxPow = 100;
    [Tooltip("ゲーム中に動くPowブロックの数")]
    private int _pow;
    [Tooltip("Powの数をカウントするテキスト")]
    private Text _powCount;
    [Tooltip("GameOverになった際キャンバスの表示")]
    private GameObject _gameOverCanvas;
    [Tooltip("チェックポイントを参照する変数")]
    GameObject[] _chackObj;
    [Tooltip("チェックポイントリスト")]
    List<GameObject> _chackList = new List<GameObject>();
    [Header("ヒエラルキーの上から順番にチェックポイントをカウント(0スタート)")]
    [Tooltip("チェックポイントのカウント")]
    public int _chackCount = 0;
    [Tooltip("PlayerのTransform")]
    Transform _playerTrans;
    [Tooltip("Scoreの増減")]
    GameObject _plasMinas;
    [Tooltip("ゲーム中")]
    public bool _playGame;
    //外から変数の中身を変えることがなかったら下の文に変更します。
    //public bool _playGame => _play;
    [Tooltip("チェックポイント通過時のエフェクト")]
    [SerializeField] GameObject _collect;

    //シーン引き継いでも消えないぞ！
    protected override bool _dontDestroyOnLoad { get { return true; } }

    /// <summary>一番初めにやってほしいことはここに書く。</summary>
    void Start()
    {
        _gameOverCanvas = transform.GetChild(0).gameObject;
        _gameOverCanvas.SetActive(false);
        //powブロックの初期化
        PowReset();
        Begin();
    }

    /// <summary>ロードする度、初めにやってほしいことのメソッド。</summary>
    void Begin()
    {
        //powの残り個数をカウントするテキストの参照
        _powCount = GameObject.Find("Count")?.GetComponent<Text>();
        //テキストの表示
        ShowText(_powCount);
        //スタート地点とチェックポイントを全部参照
        _chackObj = GameObject.FindGameObjectsWithTag("Respawn");
        //_chackObjのnull判定
        if (_chackObj.Length != 0)
        {
            //参照したスタート地点とチェックポイントをリストに追加
            foreach (var i in _chackObj)
            {
                _chackList.Add(i);
            }
        }
        //playerのTransformを参照
        _playerTrans = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();
        _plasMinas = GameObject.Find("minas");
        if (_plasMinas) _plasMinas.SetActive(false);
        _playGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Rキーを押すかplayerのyのポジションが-10未満なら
        if(Input.GetKeyDown(KeyCode.R) || _playerTrans.position.y < -10f)
        {
            //Powの個数を減らしてチェックポイントにワープさせる
            AddPow(-5);
            ChackPointWarp();
        }
    }

    /// <summary>Powブロックの残り個数の表示</summary>
    public void ShowText(Text powcount)
    {
        //null判定
        if (powcount)
        {
            //テキストに残り個数の表示
            powcount.text = _pow.ToString("000");
        }
    }

    /// <summary>Powブロックの数を増減させる。</summary>
    /// <param name="add">増減させる値</param>
    public void AddPow(int add)
    {
        //ブロックの数を増減させてからテキストに表示。
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

    /// <summary>残ったPowブロックの数によって評価を表示する</summary>
    /// <returns>Rankを返す</returns>
    public string Rank()
    {
        //50以上でRankS、30以上でRankA、10以上でRankB、それ以下でRankC
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

    /// <summary>PlayerをChackpointにワープさせる</summary>
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

    /// <summary>Powブロックの初期化をする</summary>
    public void PowReset()
    {
        _pow = _maxPow;
    }

    private void OnLevelWasLoaded(int level)
    {
        _playGame = false;
        //Chackpointのリストを全部消去させる。
        _chackList.Clear();
        _chackCount = 0;
        //ロードする度メソッドを呼び出す。
        Begin();
    }
}
