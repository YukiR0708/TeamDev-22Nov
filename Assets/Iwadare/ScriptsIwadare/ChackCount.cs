using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChackCount : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance._chackCount++;
            GameManager.Instance.ChackpointGet();
            gameObject.SetActive(false);
        }
    }

}
