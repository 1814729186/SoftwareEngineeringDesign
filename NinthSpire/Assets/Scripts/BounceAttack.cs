using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAttack : MonoBehaviour
{
    public int Power;
    public Vector3 Speed;
    public LayerMask WhatIsEnemy;
    public LayerMask WhatIsAbsorb;
    public GameObject BounceAnimation;

    // Start is called before the first frame update
    void Start()
    {
        AudioManger.Instance.PlayAudio("hero_land_hard", transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Time.deltaTime);
    }
    IEnumerator Fading()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player"&&collision.tag != "default")
        {
            StartCoroutine(Fading());
            Destroy(this.gameObject);
            Instantiate(BounceAnimation, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().BeAttack(1);
            }
        }
    }
}
