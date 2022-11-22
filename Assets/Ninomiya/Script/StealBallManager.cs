using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealBallManager : MonoBehaviour
{
    [SerializeField] GameObject _target;
    Vector3 vec;
    CircleCollider2D _cc;
    [SerializeField] float _notCollider;
    [SerializeField] AudioClip _audio;
    AudioSource _audioS;
    // Start is called before the first frame update
    void Start()
    {
        vec = _target.transform.position;
        _audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateA();
    }
    public void RotateA()
    {
        transform.RotateAround(vec, Vector3.forward, 30 * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _cc = GetComponent<CircleCollider2D>();
            _audioS.PlayOneShot(_audio);
            StartCoroutine("Steal");
        }
    }
    IEnumerator Steal()
    {
        _cc.enabled = false;
        yield return new WaitForSeconds(_notCollider);
        _cc.enabled = true;
    }
}
