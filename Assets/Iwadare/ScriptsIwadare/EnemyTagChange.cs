using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTagChange : MonoBehaviour
{
    Rigidbody2D _rb;
    [Tooltip("カメラの移り具合に生じて値を変える変数")]
    int _num = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //最初はsimulatedをfalseにして、メインカメラに写ったらsimulatedをtrueにする。
        _rb.simulated = false;
        //タグも一応初期化する。
        tag = "cannotDestroy";
    }

    void Update()
    {
        TagChange(_num);
        _num = 0;
    }

    ///この処理だとSceneCameraが映りこんだ時も同じ処理をしてしまうので別の処理で試した。
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

    /// <summary>Cameraの映り具合に生じてTagを変える処理</summary>
    /// <param name="num">カメラの移り具合に生じて値を変える変数</param>
    private void TagChange(int num)
    {
        switch(num)
        {
            //どのカメラにも映っていなかったら敵を倒せなくする。
            case 0:　
                tag = "cannotDestroy"; 
                break;
            //SceneCameraのみ映っていたら敵を倒せなくする。
            case 1:
                tag = "cannotDestroy";
                break;
            //Main Cameraのみ映っていたら敵を倒せるようにする。
            case 2:
                if(!_rb.simulated)_rb.simulated = true;
                tag = "ShakeDown";
                break;
            //両方に移っていたら敵を倒せるようにする。
            case 3:
                if(!_rb.simulated)_rb.simulated = true;
                tag = "ShakeDown";
                break;
        }
    }
}
