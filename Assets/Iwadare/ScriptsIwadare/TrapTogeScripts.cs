using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�g�Q�ɓ�����������Pow�u���b�N�����炵��Player�����[�v������X�N���v�g</summary>
public class TrapTogeScripts : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�g�Q���v���C���[�ɓ���������APow�u���b�N�����炵�ē���̃`�F�b�N�|�C���g�փ��[�v������B
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddPow(-5);
            GameManager.Instance.ChackPointWarp();
        }
    }
}
