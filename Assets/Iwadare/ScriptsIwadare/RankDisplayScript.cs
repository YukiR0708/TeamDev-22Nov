using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankDisplayScript: MonoBehaviour
{
    [SerializeField] GameObject _rankCanvas;

    
    private void OnDisable()
    {
        Debug.Log("死んだわオレ");
        Instantiate(_rankCanvas, transform.position, Quaternion.identity);
    }
}
