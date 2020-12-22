using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer:MaZhongping
/// Describe：控制箭头的上下移动
/// </summary>
public class CursorUD : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("移动速度")]
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float border = 0.5f;//移动边界
    //控制交投上下移动，上慢下快
    float thisY;    //初始位置y轴坐标
    bool state;
    void Start()
    {
        thisY = transform.position.y;
        state = true;   //向上运动
    }

    // Update is called once per frame
    void Update()
    {
        //控制箭头上下运动
        if (state)
        {
            this.transform.position += new Vector3(0,speed,0)*Time.deltaTime;
        }
        else
        {
            this.transform.position += new Vector3(0, -1*speed, 0) * 2*Time.deltaTime;
        }
        if (this.transform.position.y > thisY + border) state = false;
        else if (this.transform.position.y < thisY - border) state = true;
    }
}
