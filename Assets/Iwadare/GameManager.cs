using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonovihair<GameManager>
{
    [Tooltip("PowƒuƒƒbƒN‚Ì”")]
    int _pow = 100;

    private Text Count;
    protected override bool _dontDestroyOnLoad { get { return true; } }

    // Start is called before the first frame update
    void Start()
    {
        Begin();
    }

    void Begin()
    {
        Count = GameObject.Find("Count").GetComponent<Text>();
        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowText()
    {
        Count.text = _pow.ToString("000");
    }
    public void AddPow(int add)
    {
        _pow += add;
        ShowText();
    }

    public string Rank()
    {
        if(_pow > 50)
        {
            return "Rank S";
        }
        else if(_pow > 30)
        {
            return "Rank A";
        }
        else if(_pow > 10)
        {
            return "Rank B";
        }
        else
        {
            return "Rank C";
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        Begin();
    }
}
