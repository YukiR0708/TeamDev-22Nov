using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuleetManager : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] GameObject _impact;
    // Start is called before the first frame update
    void Start()
    {
        GameObject _player = GameObject.Find("Player");
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        if(_player)
        {
            Vector2 vec = _player.transform.position - this.transform.position;
            _rb.velocity = vec.normalized * _speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 3f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Instantiate(_impact, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
