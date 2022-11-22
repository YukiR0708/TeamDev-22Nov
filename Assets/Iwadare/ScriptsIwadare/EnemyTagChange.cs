using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTagChange : MonoBehaviour
{
    Rigidbody2D _rb;
    [Tooltip("�J�����̈ڂ��ɐ����Ēl��ς���ϐ�")]
    int _num = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //�ŏ���simulated��false�ɂ��āA���C���J�����Ɏʂ�����simulated��true�ɂ���B
        _rb.simulated = false;
        //�^�O���ꉞ����������B
        tag = "cannotDestroy";
    }

    void Update()
    {
        TagChange(_num);
        _num = 0;
    }

    ///���̏�������SceneCamera���f�肱�񂾎����������������Ă��܂��̂ŕʂ̏����Ŏ������B
    //private void OnBecameVisible()
    //{
    //    tag = "ShakeDown";
    //}

    //private void OnBecameInvisible()
    //{
    //    tag = "cannotDestroy";
    //}


    private void OnWillRenderObject()
    {
        if(Camera.current.name == "SceneCamera")
        {
            _num++;
        }
        if(Camera.current.name == "Main Camera")
        {
            _num += 2;
        }
    }

    /// <summary>Camera�̉f���ɐ�����Tag��ς��鏈��</summary>
    /// <param name="num">�J�����̈ڂ��ɐ����Ēl��ς���ϐ�</param>
    private void TagChange(int num)
    {
        switch(num)
        {
            //�ǂ̃J�����ɂ��f���Ă��Ȃ�������G��|���Ȃ�����B
            case 0:�@
                tag = "cannotDestroy"; 
                break;
            //SceneCamera�̂݉f���Ă�����G��|���Ȃ�����B
            case 1:
                tag = "cannotDestroy";
                break;
            //Main Camera�̂݉f���Ă�����G��|����悤�ɂ���B
            case 2:
                if(!_rb.simulated)_rb.simulated = true;
                tag = "ShakeDown";
                break;
            //�����Ɉڂ��Ă�����G��|����悤�ɂ���B
            case 3:
                if(!_rb.simulated)_rb.simulated = true;
                tag = "ShakeDown";
                break;
        }
    }
}
