using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [Tooltip("Goalした際のキャンバスの表示")]
    private GameObject _goalCanvas;
    [Tooltip("GoalCanvasのPowブロックの数を表示するテキスト")]
    private Text _goalPowCount;

    private void Start()
    {
        _goalCanvas = transform.GetChild(0).gameObject;
        _goalCanvas.SetActive(false);
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

