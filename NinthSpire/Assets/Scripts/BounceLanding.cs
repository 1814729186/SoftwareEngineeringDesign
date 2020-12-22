using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLanding : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo animInfo;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        anim.Play("Bounce");
        AudioManger.Instance.PlayAudio("hero_parry", transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!(animInfo.IsName("Bounce") && animInfo.normalizedTime < 1.0f))
        {
            Destroy(this.gameObject);
        }
    }
}
