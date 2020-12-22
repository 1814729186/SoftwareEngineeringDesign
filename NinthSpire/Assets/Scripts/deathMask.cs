using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathMask : MonoBehaviour
{
    // Start is called before the first frame update
    bool deathAllow = false;
    void Start()
    {
        this.GetComponent<UnityEngine.Canvas>().enabled = false;
        StartCoroutine(allowDeath());
    }

    // Update is called once per frame
    void Update()
    {
        if (deathAllow == false)
            this.GetComponent<UnityEngine.Canvas>().enabled = false;

        /*if (this.GetComponent<UnityEngine.Canvas>().enabled)
            if (Input.GetKey(KeyCode.K))
                this.GetComponent<UnityEngine.Canvas>().enabled = false;*/
    }
    IEnumerator allowDeath()
    {
        yield return new WaitForSeconds(1.0f);
        deathAllow = true;
    }
}
