using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuleet : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] float _shotspeed;
    [SerializeField] Vector2 _vec = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _vec * _shotspeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
