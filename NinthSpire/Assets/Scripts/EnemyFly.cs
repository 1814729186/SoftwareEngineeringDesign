using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer:MaZhongping
/// Describe：反击蝇，敌人脚本
/// </summary>
public class EnemyFly : Enemy
{
    [Header("EnemyFly参数")]
    [SerializeField] protected bool attackState;  //攻击模式
    [SerializeField] protected int flyBor;          //随机移动边界
    [SerializeField] protected int attackBor;     //攻击检测边界
    protected System.Random random;
    protected GameObject player;
    [SerializeField] protected Vector3 aimPoint;   //下一个目标移动点
    protected override void Start()
    {
        attackState = false;
        random = new System.Random();
        aimPoint = this.transform.position;
        player = GameObject.Find("Player");

        bornPosition = this.transform.position;   //当前位置作为重生点
        isAlive = true;
        moveSpeed = MAXMOVESPEED;
        animator = this.GetComponent<Animator>();
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        config = GameObject.Find("Config");
        material = GetComponent<SpriteRenderer>().material;
    }
    protected override void Move()    //重写Move方法
    {
        
        //Fly在此情况下由movePosition[0]给出中心点坐标,随机产生下一个坐标点的坐标
        if(attackState)//攻击模式下,冲向主角
        {  
            aimPoint = player.transform.position;
        }
        Vector2 temp = Vector2.MoveTowards(transform.position, aimPoint, moveSpeed);    //移向目标点
        if (temp.x - this.transform.position.x > 0) this.transform.rotation = new Quaternion(0, 0, 0, 0);
        else this.transform.rotation = new Quaternion(0, 180, 0, 0);
        GetComponent<Rigidbody2D>().MovePosition(temp);//将刚体移动到上个函数返回的目标位置上去
        if (ManHaDistance((Vector2)transform.position, aimPoint) <= 0.1f && attackState == false)   //到达当前位置，设置下一个坐标位置
        {
            //非攻击模式下，随机产生坐标点作为目标点
            int x = random.Next(2 * flyBor) - flyBor;
            int y = random.Next(2 * flyBor) - flyBor;
            aimPoint = movePosition[0].transform.position + new Vector3(x, y, 0);
        }
    }

    protected virtual void changeAttackState()
    {
        if(ManHaDistance((Vector2)transform.position, player.transform.position) <= attackBor)
        {
            attackState = true;
        }
        else
        {
            attackState = false;
        }
    }

    protected override void Update()    //重写update方法
    {
        if (isAlive)
            Move();
        if (isAlive && HP <= 0)
            Death();    //HP归零，死亡
        //攻击检测
        changeAttackState();
    }


}
