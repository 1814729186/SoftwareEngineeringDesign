using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInsect : EnemyFly
{
    [Header("FlyingInsect参数")]
    [SerializeField] private float frequency = 1.0f; //吐酸液频率
    [SerializeField] GameObject acid;   //酸液的游戏物体
    //酸液飞虫不涉及碰撞伤害，飞行方式与飞蝇相同
    [SerializeField]private float spentTime = 0.0f;
    //重写Move方法
    protected override void Move()
    {
        //Fly在此情况下由movePosition[0]给出中心点坐标,随机产生下一个坐标点的坐标
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
    protected override void Start()
    {
        attackState = false;
        random = new System.Random();
        aimPoint = this.transform.position;
        player = GameObject.Find("Player");

        bornPosition = this.transform.position;   //当前位置作为重生点
        isAlive = true;
        moveSpeed = MAXMOVESPEED;
        hurtTime = 3.0f;
        animator = this.GetComponent<Animator>();
        HP = 30;
        this.GetComponent<Rigidbody2D>().gravityScale = 0;

        frequency = 1.0f;
        config = GameObject.Find("Config");
    }


    protected override void Update()
    {
        if (isAlive)
        {
            //攻击频率检测
            spentTime += Time.deltaTime;
            if (spentTime > frequency){
                Attack();
                spentTime = 0.0f;
            } 
            Move();
        }
        if (isAlive && HP <= 0)
            Death();    //HP归零，死亡
        //攻击检测
        changeAttackState();
    }
    private void Attack()
    {  
        Instantiate(acid, this.transform.position, new Quaternion(0, 0, 0, 0));  //发出酸液
    }

}
