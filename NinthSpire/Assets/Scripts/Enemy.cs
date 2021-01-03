using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer：马忠平
/// Describe：所有Enemy的父类,定义为保护权限以上虚方法，支持重写
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("基本参数")]
    [SerializeField] protected int HP;        //怪物初始血量
    [SerializeField] protected int attack;    //怪物攻击倍率
    [SerializeField] protected Vector3 bornPosition;  //怪物出生点/重生点
    [SerializeField] protected float rebornTime;         //重生时间/秒
    [SerializeField] protected bool isAlive = true;   //是否生存
    [SerializeField] protected float hurtTime;    //受伤持续时间


    [Header("特殊参数")]
    [SerializeField] protected GameObject[] movePosition;  //循环移动爬动坐标点，使用空物体进行赋予
    [SerializeField] protected int curPosition = 0;   //当前坐标索引 
    [SerializeField] protected float MAXMOVESPEED;    //最大移动速度基准值
    [SerializeField] protected float moveSpeed;     //移动速度
    public Animator animator; //动画状态机
    public GameObject hitEffect;    //被击中效果
    protected GameObject config;
    protected Material material;

    protected virtual void Move()
    {
        //沿给定坐标点进行插值移动
        //插值得到要移动到dest位置的下一次坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, movePosition[curPosition].transform.position, moveSpeed);
        if (temp.x - this.transform.position.x > 0) this.transform.rotation = new Quaternion(0, 0, 0, 0);
        else this.transform.rotation = new Quaternion(0, 180, 0, 0);
        GetComponent<Rigidbody2D>().MovePosition(temp);//将刚体移动到上个函数返回的目标位置上去
        if (ManHaDistance((Vector2)transform.position, movePosition[curPosition].transform.position) <= 0.1f)   //到达当前位置，设置下一个坐标位置
        {
            curPosition = (curPosition + 1) % movePosition.Length;
            //设置动画方向
            Vector2 direcVec = (Vector2)movePosition[curPosition].transform.position - (Vector2)this.transform.position;
        }
    }
    protected virtual float ManHaDistance(Vector2 v1, Vector2 v2)
    {
        float res = 0;
        res = Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
        return res;
    }
    public virtual void Death()
    {

        //禁止移动，播放死亡动画，启用重力控制
        animator.Play("Die");   //播放死亡动画
        //玩家获得金钱
        config.GetComponent<Config>().coinNum++;

        isAlive = false;    //设置死亡状态
        Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 1; //设置重力
        this.GetComponent<BoxCollider2D>().enabled = false; //取消碰撞体
                                                            //启动死亡协程
        StartCoroutine(DeathIEnu());
        //调用重生程序
        Reborn();

    }

    public virtual void Reborn()
    {


        StartCoroutine(RebornIEnu());
    }  //怪物重生控制

    protected virtual IEnumerator DeathIEnu() //死亡协程
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        yield return new WaitForSeconds(3.0f);
        //死亡协程，等待动画播放完毕，并逐渐隐去当前怪物
        this.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        this.GetComponent<SpriteRenderer>().enabled = false;    //取消渲染
        this.GetComponent<Rigidbody2D>().gravityScale = 0; //取消重力
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    protected virtual IEnumerator RebornIEnu() //重生协程
    {
        //重生协程等待重生时间后重生
        yield return new WaitForSeconds(rebornTime);
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        this.transform.position = bornPosition; //回到出生点
        animator.Play("Move");
        curPosition = 0;
        this.GetComponent<SpriteRenderer>().enabled = true;    //启动渲染
        this.GetComponent<BoxCollider2D>().enabled = true;      //启动碰撞体
        isAlive = true;
        HP = 2;
    }


    protected virtual void Start()
    {
        bornPosition = this.transform.position;   //当前位置作为重生点
        isAlive = true;
        moveSpeed = MAXMOVESPEED;
        hurtTime = 3.0f;
        animator = this.GetComponent<Animator>();
        HP =2;
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        config = GameObject.Find("Config");
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isAlive)
            Move();
        if (isAlive && HP <= 0)
            Death();    //HP归零，死亡
    }

    public virtual void BeAttack(int value)    //受到伤害，参数为受伤值
    {
        //受伤时，移动速度减慢，设置协程
        HP -= value;
        StartCoroutine(BeAttackIEnu());   //启动协程
    }
    public virtual IEnumerator BeAttackIEnu()  //受伤协程
    {
        AudioManger.Instance.PlayAudio("hero_land_hard", transform.position);
        //受伤渲染变淡，速度减慢
        moveSpeed = MAXMOVESPEED / 2;
        //闪烁效果
        GameObject hitHandle = Instantiate(hitEffect, transform.position, transform.rotation);
        material.SetColor("_FlashColor", Color.white);
        for (int i = 1; i < 20; i++)
        {
            material.SetFloat("_FlashAmount", 0.05f * i);
            yield return new WaitForSeconds(0.01f);
        }
        material.SetFloat("_FlashAmount", 0f);
        //this.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Destroy(hitHandle);
        yield return new WaitForSeconds(hurtTime);
        //this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);  //设置原版显示
        moveSpeed = MAXMOVESPEED;   //速度回复
    }

    //攻击
    /*public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().beAttack(this.transform, attack);
        }

    }*/

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().beAttack(this.transform, attack);
        }
    }
}
