using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerDaMo : NPCController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        talk = new string[] { "嘿，矮个子朋友，似乎你刚来镇上？",
                                    "嘿，别垂头丧气，最近上面闹腾地厉害，我得去看看，免得老家伙总是讽刺我。",
                                    "准备好行装再上路，如果你也有兴趣的话，想上去的话可以乘电梯。"
           };
        random = new System.Random((int)Time.time);
        player = GameObject.Find("Player");
        name = "大漠：";
    }
}
