using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankDisplayScript: MonoBehaviour
{
    [SerializeField] GameObject _rankCanvas;
    [SerializeField] GameObject _sound;
    
    private void OnDisable()
    {
        Debug.Log("���񂾂�I��");
        Instantiate(_rankCanvas, transform.position, Quaternion.identity);
        _sound.SetActive(false);
    }
}
