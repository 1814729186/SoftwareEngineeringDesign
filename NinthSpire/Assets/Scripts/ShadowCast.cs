using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCast : MonoBehaviour
{

    public LayerMask whatIsDistrict, whatIsBreakable, whatIsWall;
    public float speed;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        AudioManger.Instance.PlayAudio("hero_tentacle_sword", transform.position);
        StartCoroutine(DestroyIEnu());
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
    }

    IEnumerator DestroyIEnu()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit : " + collision.gameObject.layer);
        //Debug.Log(collision.gameObject.layer);
        //Debug.Log(whatIsBreakable.value);
        if ((whatIsBreakable.value >> collision.gameObject.layer & 1) != 0)
        {
            Destroy(collision.gameObject);
        }
        else if ((whatIsDistrict.value >> collision.gameObject.layer & 1) != 0)
           // || (whatIsWall.value >> collision.gameObject.layer & 1) != 0)
        {
            //Debug.Log("Hit District");
            Destroy(this.gameObject);
        }
        if (collision.tag == "Enemy")    //击中敌人
        {

            collision.GetComponent<Enemy>().BeAttack((int)power);
            Destroy(this.gameObject);
        }
    }

    void SetSpriteRenderer(SpriteRenderer render,float value)
    {
        render.color = new Color(value, value, value, value);
    }
        
 }
