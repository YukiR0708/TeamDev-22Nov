using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowAddScript : MonoBehaviour
{
    [SerializeField] GameObject _effect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddPow(1);
            Instantiate(_effect,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
