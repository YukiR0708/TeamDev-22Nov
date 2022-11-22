using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [Tooltip("Goal�����ۂ̃L�����o�X�̕\��")]
    private GameObject _goalCanvas;
    [Tooltip("GoalCanvas��Pow�u���b�N�̐���\������e�L�X�g")]
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

