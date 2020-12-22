using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("生命值")]
    public float HP;
    [Header("速度参数")]
    [SerializeField]private float walkSpeed;        //移动速度
    [SerializeField]private float recoilSpeed;      //反冲速度
    [SerializeField] private float hurtRecoilForce;
    [SerializeField]private float upRecoilForce;    //向上反冲乘数因子
    [SerializeField]private float jumpForce;        //跳跃初始速度
    [SerializeField]private float jumpHoldForce;    //跳跃长按增加速度
    [SerializeField]private float maxDown;          //最大下降速度

    [Header("基准参数")]
    [SerializeField]private Transform feetpos;      //玩家脚部基准点
    [SerializeField]private float checkRadius;      //基准点检查半径
    [SerializeField]private LayerMask whatIsGround; //地面层掩码
    [SerializeField]private LayerMask whatIsTrap;   //陷阱层掩码
    [SerializeField] public Vector3 rebornPosition; //重生点

    [Header("时间参数")]
    [SerializeField]private float jumpTime;         //跳跃最大长按时间
    [SerializeField]private float[] castTime;       //蓄力完成时间（分阶段）
    [SerializeField]private float floatingTime;     //蓄力释放后的悬浮时间
    [SerializeField]private float bounceCooldown;   //反冲的冷却时间

    [Header("预设动作/特效资源")]
    [SerializeField]private GameObject[] boostingEffects;           //蓄力特效预设对象
    [SerializeField]private GameObject[] castThreshAnimations;      //蓄力动画预设对象
    [SerializeField]private GameObject[] castLoopingAnimations;     //蓄力完成循环动画预设对象
    [SerializeField]private GameObject[] castReleaseAnimations;     //技能动画预设对象
    [SerializeField]private GameObject[] bounceAnimations;         //反冲动画预设对象
    [SerializeField]private GameObject hurtMask;                    //受伤动画预设对象
    private GameObject deathMask;                   //死亡动画预设对象

    private Config config;

    private bool disableAll;            //不响应键盘输入
    private Rigidbody2D rb;             //2D刚体组件句柄
    private Animator anim;              //动画机句柄
    private AnimationClip[] clips;      //动画机的所有动画片段

    private float moveInput;            //用户输入的左右方向标

    private bool isGrounded;            //在地面标记
    private bool isJumping;             //在跳跃长按过程中标记
    private bool isCasting;             //在蓄力中标记
    private int isCastActive;           //分阶段技能蓄力完毕标记

    private float jumpTimerCounter;     //跳跃长按计时器（倒计时）
    private float castTimeCounter;      //蓄力计时器（正计时）
    private float bounceTimerCounter;   //反冲计时器（倒计时）

    private int direct;                 //玩家当前朝向（0-左 1-右）
    private float trueSpeedX;           //下一帧玩家X轴速度
    private float trueSpeedY;           //下一帧玩家Y轴速度

    private GameObject inDeath;         //死亡动画预设实例句柄
    private GameObject effect;          //蓄力特效预设实例句柄
    private GameObject threshAnim;      //蓄力动画预设实例句柄
    private GameObject releaseAnim;     //技能动画预设实例句柄
    private Vector2[] castSize;         //技能贴图长度
    private AudioSource playSource;     //音效实例句柄
    private bool deathAllow = false;
    public int castAbility;
    // Start is called before the first frame update
    void Start()
    {
        deathMask = GameObject.Find("DeathMask");
        rebornPosition = this.transform.position;   //初始重生位置
        config = GameObject.Find("Config").GetComponent<Config>();
        rb = GetComponent<Rigidbody2D>();       //获取刚体组件
        anim = GetComponent<Animator>();        //获取动画机组件
        clips = anim.runtimeAnimatorController.animationClips;      //读取动画机动画片段
        HP = config.Health;
        //建立初始状态
        transform.position = config.InitPosition;
        transform.rotation = Quaternion.Euler(config.InitRotation);
        anim.Play("Idle");      //玩家“闲置”
        direct = config.InitRotation.y > 90 ? -1 : 1;   //朝向
        castAbility = config.CastAbility;
        castSize = new Vector2[5];
        for (int i = 0; i < castReleaseAnimations.Length; ++i)
        {
            castSize[i] = castReleaseAnimations[i].GetComponent<Renderer>().bounds.size;     //获取技能贴图长度
        }
        isCastActive = 0;       //技能蓄力标记置0（表示没有蓄力技能已准备好）
        deathAllow = true;
        GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f) ;
        playSource = AudioManger.Instance.PlayAudio("focus_health_charging", transform, 1);
        playSource.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableAll)    //如果不响应键盘输入，则直接跳过
        {
            Prepare();      //准备阶段
            Move();         //处理移动输入
            Jump();         //处理跳跃输入
            Cast();         //处理蓄力输入
        }
        Modify();           //更新下一帧
        if (HP <= 0&&deathAllow == true)
            Death();
    }
    public void Death()
    {
        deathMask.GetComponent<UnityEngine.Canvas>().enabled = true;
        StartCoroutine(Reborn());
    }
    //准备阶段
    public void Prepare()
    {
        //读入刚体速度参数
        trueSpeedX = rb.velocity.x;
        trueSpeedY = rb.velocity.y;
    }

    //处理移动输入
    public void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");     //获取水平方向输入
        if (moveInput > 0)  //键盘输入方向向右
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            direct = 1;     //修改朝向向右
        }
        else if (moveInput < 0)  //键盘输入方向向左
        {
            transform.eulerAngles = new Vector3(0, 180, 0);     //镜像贴图
            direct = -1;     //修改朝向向左
        }
        trueSpeedX = moveInput * walkSpeed;     //确定移动速度
    }

    //处理跳跃输入
    public void Jump()
    {
        //检测玩家是否落地
        isGrounded = Physics2D.OverlapCircle(feetpos.position, checkRadius, whatIsGround);

        //落地状态且按下跳跃键
        if (isGrounded == true && Input.GetKeyDown(KeyCode.K))
        {
            isJumping = true;   //启用跳跃长按
            jumpTimerCounter = jumpTime;    //跳跃长按倒计时启动
            trueSpeedY = jumpForce;         //设置初始跳跃速度
            anim.Play("Jump");              //播放起跳动画
        }

        //处理跳跃长按
        if (isJumping == true && Input.GetKey(KeyCode.K))
        {
            //跳跃长按倒计时未归零，响应跳跃长按
            if (jumpTimerCounter > 0)
            {
                trueSpeedY += jumpHoldForce;            //增加跳跃速度
                jumpTimerCounter -= Time.deltaTime;     //递减倒计时
            }
            //跳跃长按超时，停止响应跳跃长按
            else
            {
                isJumping = false;
            }
        }
        //跳跃键抬起，终止跳跃状态
        if (isJumping == true && Input.GetKeyUp(KeyCode.K))
        {
            isJumping = false;
        }
    }

    //处理蓄力输入
    public void Cast()
    {
        bounceTimerCounter -= Time.deltaTime;    //更新反冲冷却计时器
        if (Input.GetKeyDown(KeyCode.J))
        {

            if (Input.GetKey(KeyCode.S) && bounceTimerCounter < 0f)    //反冲攻击
            {
                //Debug.Log("Land a down atk");
                //
                //反冲相关代码
                //Instantiate(bounceAnimations[0], feetpos.position, Quaternion.Euler(0f, 0f, 0f));
                if (!isGrounded)
                {
                    bounceTimerCounter = bounceCooldown;    //启动反冲冷却计时器
                    Instantiate(bounceAnimations[1], feetpos.position, Quaternion.Euler(0f, 0f, 90f));
                    RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1f), 0, Vector2.down, 1.2f, whatIsTrap);
                    if (hit2D)
                    {
                        Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.red, 6.0f);
                        trueSpeedY = recoilSpeed * upRecoilForce;
                    }
                }
            }
            if (castAbility > 0)
            {
                isCasting = true;               //开始蓄力
                castTimeCounter = 0;            //重置蓄力时间为0
                isCastActive = 0;               //重置生效蓄力段数
                playSource = AudioManger.Instance.PlayAudio("focus_health_charging", transform, 1);
                //播放粒子吸收特效
                effect = Instantiate(boostingEffects[0], transform.position, transform.rotation);
                //播放蓄力动画
                threshAnim = Instantiate(castThreshAnimations[0], this.transform.position, this.transform.rotation);
            }
        }
        if (isCasting && Input.GetKey(KeyCode.J))
        {
            //特效跟随
            if (effect != null)
                effect.transform.position = this.transform.position;
            //蓄力动画跟随
            if (threshAnim != null)
                threshAnim.transform.position = this.transform.position;
            //更新计时器
            castTimeCounter += Time.deltaTime;
            //蓄力阶段循环检测和特效动态更新
            for (int i = castAbility - 1; i >= 0; --i)
            {
                if (castTimeCounter > castTime[i])
                {
                    if (isCastActive < i + 1)
                    {
                        //i级蓄力生效
                        isCastActive = i + 1;
                        //停止粒子吸收特效
                        Destroy(effect);
                        if (i == castAbility - 1)
                        {
                            //实例化循环粒子特效
                            effect = Instantiate(castLoopingAnimations[i], this.transform.position, this.transform.rotation);
                        }
                        else
                        {
                            //播放新粒子吸收特效
                            effect = Instantiate(boostingEffects[i + 1], transform.position, transform.rotation);
                            //停止蓄力动画
                            Destroy(threshAnim);
                            //播放新蓄力动画
                            threshAnim = Instantiate(castThreshAnimations[i + 1], this.transform.position, this.transform.rotation);
                        }
                        break;
                    }
                }
            }
        }
        if (isCasting && Input.GetKeyUp(KeyCode.J))
        {
            isCasting = false;      //蓄力终止
            playSource.Stop();      //音效终止
            //销毁粒子特效及动画
            if (effect != null)
                Destroy(effect);
            if (threshAnim != null)
                Destroy(threshAnim);

            //如果一段蓄力生效
            if (isCastActive == 1)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    releaseAnim = Instantiate(castReleaseAnimations[0], this.transform.position + new Vector3(0, castSize[0].x, 0), Quaternion.Euler(0, (direct + 1) * 90, -90));
                    //向上生成暗影波发出，根据左右方向生成暗影波发出
                    //无反作用力
                    trueSpeedX = trueSpeedY = 0;    //技能释放时玩家静滞
                }
                else
                {
                    releaseAnim = Instantiate(castReleaseAnimations[0], this.transform.position + new Vector3(direct * castSize[0].x, 0, 0), Quaternion.Euler(0, (direct + 1) * 90, 0));
                    //根据左右方向生成暗影波发出
                    trueSpeedX = -direct * recoilSpeed; //反作用力
                    trueSpeedY = 0;
                }
                StartCoroutine(Floating(floatingTime));
            }
            //如果二段蓄力生效
            else if (isCastActive == 2)
            {
                Instantiate(castReleaseAnimations[1], this.transform.position + new Vector3(1 * direct * castSize[1].x, castSize[1].y / 2, 0), Quaternion.Euler(0, (direct + 1) * 90, 0));
                Instantiate(castReleaseAnimations[1], this.transform.position + new Vector3(2 * direct * castSize[1].x, castSize[1].y, 0), Quaternion.Euler(0, (direct + 1) * 90, 0)).transform.localScale = new Vector3(2f, 2f, 1f);
                //从人物前上方发出水华波
                //无反作用力
                trueSpeedX = trueSpeedY = 0;    //技能释放时玩家静滞
                StartCoroutine(Floating(floatingTime));
            }
        }
    }

    //更新下一帧
    public void Modify()
    {
        //下落速度不超过阈值
        if (trueSpeedY < maxDown) trueSpeedY = maxDown;

        //载入下一帧玩家速度向量
        rb.velocity = new Vector2(trueSpeedX, trueSpeedY);

        //确定玩家是否闲置以及在地面移动
        if (!isJumping && isGrounded)
        {
            if (moveInput == 0.0f) anim.Play("Idle");
            else anim.Play("Walk");
        }
    }

    //技能硬直
    IEnumerator Floating(float floatingTime)
    {
        disableAll = true;
        yield return new WaitForSeconds(floatingTime);
        disableAll = false;
    }
    //无敌时间
    IEnumerator UnAttack(float unAttackTime)
    {
        float amount = 0.0f;
        Material material = GetComponent<SpriteRenderer>().material;
        this.tag = "default";   //修改标签，不能被敌人识别
        //闪烁效果
        GameObject hurtHandle = Instantiate(hurtMask, transform.position, transform.rotation);
        material.SetColor("_FlashColor", Color.white);
        for (int i = 1; i < 20; i++)
        {
            material.SetFloat("_FlashAmount", 0.05f * i);
            yield return new WaitForSeconds(0.01f);
        }
        material.SetColor("_FlashColor", Color.black);
        for (int i = 1; i < 20; i++)
        {
            material.SetFloat("_FlashAmount", 0.05f * i);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(hurtHandle);
        material.SetFloat("_FlashAmount", 0.3f);
        yield return new WaitForSeconds(1f);
        material.SetFloat("_FlashAmount", 0f);
        this.tag = "Player";    //改回原标签
    }
    IEnumerator Reborn()
    {
        disableAll = true;
        this.transform.position = rebornPosition;   //回到重生点
        HP = 3;
        config.Health = 3;
        yield return new WaitForSeconds(2.5f);
        disableAll = false;
        deathMask.GetComponent<UnityEngine.Canvas>().enabled = false;
    }    //死亡回调函数

    //死亡回调函数
    public void Death(bool isSet)
    {
        GameObject.Find("DeathMask").GetComponent<Canvas>().enabled = true;
        rb.velocity = new Vector2(0f, 0f);
        if (effect != null)
            Destroy(effect);
        if (threshAnim != null)
            Destroy(threshAnim);
        if (playSource != null)
            playSource.Stop();
        StartCoroutine(Reborn());
    }

    public void beAttack(Transform trans, float value)    //被攻击调用函数,首个参数为攻击者位置（设计反冲效果），第二个参数为受伤害值
    {
        HP -= value;
        config.Health -= (int)value;
        //设置僵直时间
        StartCoroutine(Floating(floatingTime));
        //无敌时间
        StartCoroutine(UnAttack(1.0f));
        //受击反冲设计
        Vector2 temp = (Vector2)(this.transform.position - trans.position).normalized;//指向主角的反冲单位向量
        trueSpeedX = temp.x * hurtRecoilForce;//反作用力
        trueSpeedY = temp.y * hurtRecoilForce;
        if(effect!=null)
            Destroy(effect);
        if(threshAnim!=null)
            Destroy(threshAnim);
        if(playSource != null)
            playSource.Stop();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Trap")
        {
            Death();
        }
    }
}
