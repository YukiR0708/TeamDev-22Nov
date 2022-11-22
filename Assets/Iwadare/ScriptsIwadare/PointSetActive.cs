using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSetActive : MonoBehaviour
{
    bool _falsetime;

    private void Update()
    {
        if(!_falsetime)
        {
            _falsetime = true;
            StartCoroutine(FalseTime());
        }
    }

    IEnumerator FalseTime()
    {
        yield return new WaitForSeconds(1f);
        _falsetime = false;
        gameObject.SetActive(false);
    }
}
