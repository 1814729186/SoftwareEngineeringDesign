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
    [SerializeField] protected int flyBor;        //随机移动边界
    [SerializeField] protected int attackBor;     //攻击检测边界
    protected System.Random random;
    protected GameObject player;
    [SerializeField] protected Vector3 aimPoint;    //下一个目标移动点
    [SerializeField] protected float retreatingTime;//回游时限
    [SerializeField] protected float wanderingTime; //游荡时限
    [SerializeField] protected float detectFactor;  //索敌范围因子
    private bool motiveFlag;
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
        motiveFlag = false;
    }
    protected override void Move()    //重写Move方法
    {
        if (!motiveFlag)
        {
            if (attackState)//攻击模式下,冲向主角
            {
                if (ManHaDistance((Vector2)transform.position, movePosition[0].transform.position) <= detectFactor * flyBor)
                {
                    aimPoint = player.transform.position;//StartCoroutine(Retreat(false));
                }
                else
                {
                    StartCoroutine(Motivate(movePosition[0].transform.position, retreatingTime));
                }
            }
            else    //Fly在此情况下由movePosition[0]给出中心点坐标,随机产生下一个坐标点的坐标
            {
                //非攻击模式下，随机产生坐标点作为目标点
                int x = random.Next(2 * flyBor) - flyBor;
                int y = random.Next(2 * flyBor) - flyBor;
                aimPoint = movePosition[0].transform.position + new Vector3(x, y, 0);
                StartCoroutine(Motivate(aimPoint, wanderingTime));
            }
        }
        float tmpX = transform.position.x;
        Vector2 temp = Vector2.MoveTowards(transform.position, aimPoint, moveSpeed);    //移向目标点
        if (temp.x - tmpX < 0) this.transform.rotation = Quaternion.Euler(0, 0, 0);
        else this.transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody2D>().MovePosition(temp);//将刚体移动到上个函数返回的目标位置上去
    }

    IEnumerator Motivate(Vector3 aim, float motivatedTime)
    {
        motiveFlag = true;
        aimPoint = aim;
        yield return new WaitForSeconds(motivatedTime);
        motiveFlag = false;
    }

    protected virtual void changeAttackState()
    {
        if(!attackState && ManHaDistance((Vector2)transform.position, player.transform.position) <= attackBor)
        {
            AudioManger.Instance.PlayAudio("Miss", transform.position);
            attackState = true;
        }
        else if (ManHaDistance((Vector2)transform.position, player.transform.position) > attackBor)
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
