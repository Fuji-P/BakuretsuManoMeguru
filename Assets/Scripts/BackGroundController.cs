using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    //背景をスクロールさせるスピード
    [SerializeField] private float scrollSpeed;
    //背景のスクロールを開始する位置
    [SerializeField] private float startLine;
    //背景のスクロールが終了する位置
    [SerializeField] private float deadLine;


    void Update()
    {
//        Scroll();
    }
    void FixedUpdate()
    {
        Scroll();
    }

    public void Scroll()
    {
        //x座標をscrollSpeed分動かす
        transform.Translate(-scrollSpeed, 0, 0);

        //もし背景のx座標よりdeadLineが大きくなったら
        if(transform.position.x <= deadLine)
        {
            //背景をstartLineまで戻す
            transform.position = new Vector3(startLine, 0, 0);
        }
    }
}
