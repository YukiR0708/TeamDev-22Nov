using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float _cooltime;
    float _time;
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _shotpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _cooltime)
        {
            Instantiate(_bullet, _shotpoint.transform.position, transform.rotation);
            _time = 0;
        }
    }
}
