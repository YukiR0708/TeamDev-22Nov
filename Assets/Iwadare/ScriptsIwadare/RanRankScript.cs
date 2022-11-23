using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RanRankScript : MonoBehaviour
{
    private Text _rankText;
    private Text _countText;
    private GameObject _gameCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        //OnEnableはStartより早く動くのでAwakeでエラーを出さないようにする。
        _countText = transform.GetChild(1).gameObject.GetComponent<Text>();
        _rankText = transform.GetChild(2).gameObject.GetComponent<Text>();
        _gameCanvas = GameObject.Find("GameCanvas");
    }
    private void OnEnable()
    {
        //startに書くとうまく反映されないのでアクティブ時に反映させるようにした。
        _rankText.text = GameManager.Instance.Rank();
        GameManager.Instance.ShowText(_countText);
        GameManager.Instance._playGame = false;
        _gameCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        _gameCanvas.SetActive(true);
    }
}
