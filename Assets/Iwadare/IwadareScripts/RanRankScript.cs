using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RanRankScript : MonoBehaviour
{
    private Text _rank;
    // Start is called before the first frame update
    void Start()
    {

        _rank = GetComponent<Text>();
        //�ŏ�false��Ԃɂ��Ă�����true�ɂȂ�����rank�\���B
        _rank.text = GameManager.Instance.Rank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
