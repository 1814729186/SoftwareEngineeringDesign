using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInsect : EnemyFly
{
    [Header("FlyingInsect参数")]
    [SerializeField] private float MaxAtkTimeDelta;     //攻击时间间隔上限
    [SerializeField] private int MaxAtkRep;           //攻击重数上限
    [SerializeField] GameObject acid;   //酸液的游戏物体
    [SerializeField] float flyBorXdivY;
    [SerializeField] float atkArc;
    
    //酸液飞虫飞行方式与飞蝇相同
    private float spentTime = 0.0f; //攻击冷却计时器
    private float randAtkTime;  //随机攻击间隔
    private int randAtkRep;     //随机攻击重数


    //重写Move方法
    protected override void Move()
    {
        //Fly在此情况下由movePosition[0]给出中心点坐标,随机产生下一个坐标点的坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, aimPoint, moveSpeed);    //移向目标点
        if (temp.x - this.transform.position.x > 0) this.transform.rotation = Quaternion.Euler(0, 0, 0);
        else this.transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody2D>().MovePosition(temp);//将刚体移动到上个函数返回的目标位置上去
        if (ManHaDistance((Vector2)transform.position, aimPoint) <= 0.1f)   //到达当前位置，设置下一个坐标位置
        {
            //随机产生坐标点作为目标点
            int flyBorX = (int)((float)flyBor * flyBorXdivY);
            int x = random.Next(2 * flyBorX) - flyBorX;
            int y = random.Next(2 * flyBor) - flyBor;
            aimPoint = movePosition[0].transform.position + new Vector3(x >> 5, y >> 5, 0);
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
        animator = this.GetComponent<Animator>();
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        config = GameObject.Find("Config");
        material = GetComponent<SpriteRenderer>().material;
        randAtkTime = (float)random.NextDouble() * MaxAtkTimeDelta;
        randAtkRep = random.Next(1, MaxAtkRep);
    }


    protected override void Update()
    {
        if (isAlive)
        {
            spentTime += Time.deltaTime;
            //攻击频率检测
            if (ManHaDistance((Vector2)transform.position, player.transform.position) <= attackBor)
            {
                if (spentTime > randAtkTime)
                {
                    Attack();
                    spentTime = 0.0f;
                    randAtkTime = (float)random.NextDouble() * MaxAtkTimeDelta;
                    randAtkRep = random.Next(1, MaxAtkRep);
                }
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
        Vector3 vel = player.transform.position - transform.position;
        float angle = SignedAngleBetween(vel, Vector3.right, -Vector3.forward);
        Instantiate(acid, this.transform.position, Quaternion.Euler(0f, 0f, angle));  //发出中心瞄准酸液
        for (float tmpArc = atkArc / randAtkRep; randAtkRep > 0; randAtkRep--)
        {
            Instantiate(acid, this.transform.position, Quaternion.Euler(0f, 0f, angle + tmpArc * randAtkRep));
            Instantiate(acid, this.transform.position, Quaternion.Euler(0f, 0f, angle - tmpArc * randAtkRep));
        }

    }
    public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));
        float signed_angle = angle * sign;
        return (signed_angle <= 0) ? 360 + signed_angle : signed_angle;
    }
}
