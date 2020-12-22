using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConeController : MonoBehaviour
{
    public float gravityScale;
    public int power;
    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }
    public void drop()
    {
        GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().beAttack(this.transform,power);
        }
        if(collision.tag != "default")
            Destroy(this.gameObject);
    }
}
