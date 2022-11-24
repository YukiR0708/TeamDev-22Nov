using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [Tooltip("Goalした際のキャンバスの表示")]
    private GameObject _goalCanvas;
    [Tooltip("一番親のオブジェクト取得")]
    private GameObject _parent;
    [Tooltip("GoalCanvasのPowブロックの数を表示するテキスト")]
    private Text _goalPowCount;

    private void Awake()
    {
        _goalCanvas = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Goal();
        }
    }

    public void Goal()
    {
        GameManager.Instance._playGame = false;
        _goalCanvas.SetActive(true);
        _goalPowCount = _goalCanvas.transform.GetChild(1).gameObject.GetComponent<Text>();
        GameManager.Instance.ShowText(_goalPowCount);
    }
}

