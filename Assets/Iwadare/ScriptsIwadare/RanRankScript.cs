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
        //OnEnable��Start��葁�������̂�Awake�ŃG���[���o���Ȃ��悤�ɂ���B
        _countText = transform.GetChild(1).gameObject.GetComponent<Text>();
        _rankText = transform.GetChild(2).gameObject.GetComponent<Text>();
        _gameCanvas = GameObject.Find("GameCanvas");
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //start�ɏ����Ƃ��܂����f����Ȃ��̂ŃA�N�e�B�u���ɔ��f������悤�ɂ����B
        _rankText.text = GameManager.Instance.Rank();
        GameManager.Instance.ShowText(_countText);
        _gameCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        _gameCanvas.SetActive(true);
    }
}
