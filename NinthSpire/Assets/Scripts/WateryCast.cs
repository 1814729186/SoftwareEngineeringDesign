using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateryCast : MonoBehaviour
{

    public LayerMask whatIsDistrict, whatIsBreakable, whatIsObstacle;
    public float speed;
    public float power;
    private Animator anim;              //动画机句柄
    private AnimationClip[] clips;      //动画机的所有动画片段
    private bool animDying;             //技能释放阶段


    // Start is called before the first frame update
    void Start()
    {
        AudioManger.Instance.PlayAudio("hero_nail_art_cyclone_slash_short", transform.position);
        anim = GetComponent<Animator>();
        clips = anim.runtimeAnimatorController.animationClips;
        animDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        float tmpSpd = speed;
        if (!animDying && Physics2D.OverlapCircle(transform.position, 0.05f, whatIsObstacle))
        {
            anim.Play("WateryExplode");
            animDying = true;
        }
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animDying && animInfo.IsName("WateryExplode") && animInfo.normalizedTime > 0.99f)//WateryExplode
        {
            Destroy(this.gameObject);
        }
        transform.Translate(new Vector3(0, animDying ? 0 : - tmpSpd * Time.deltaTime, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "default")
        {

        }
        if ((whatIsBreakable.value >> collision.gameObject.layer & 1) != 0)
        {
            //Destroy(collision.gameObject);
            StartCoroutine(SlowDesIEnu(collision));
        }
        else if ((whatIsDistrict.value >> collision.gameObject.layer & 1) != 0)
        // || (whatIsWall.value >> collision.gameObject.layer & 1) != 0)
        {
            Destroy(this.gameObject);
        }
        //Debug.Log("Hit : " + collision.gameObject.layer);
        //Debug.Log(collision.gameObject.layer);
        //Debug.Log(whatIsBreakable.value);
        if (collision.tag == "Enemy")    //击中敌人
        {

            collision.GetComponent<Enemy>().BeAttack((int)power);
            Destroy(this.gameObject);
        }
    }
    IEnumerator SlowDesIEnu(Collider2D collision)
    {
        SpriteRenderer render = collision.GetComponent<SpriteRenderer>();
        SetSpriteRenderer(render, 0.6f);
        yield return new WaitForSeconds(0.2f);
        SetSpriteRenderer(render, 0f);
        yield return new WaitForSeconds(0.2f);

        Destroy(collision.gameObject);

    }
    void SetSpriteRenderer(SpriteRenderer render, float value)
    {
        render.color = new Color(value, value, value, value);
    }

}
