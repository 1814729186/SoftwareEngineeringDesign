using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeTrigger : MonoBehaviour
{
    Transform cone;
    private void Start()
    {
        cone = transform.Find("Cone");
    }
    private void Update()
    {
        if (cone == null)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(cone == null)
        {
            Destroy(this.gameObject);
        }
        if(collision.tag == "Player")
        {
            cone.GetComponent<ConeController>().drop();
            
        }
    }
}
