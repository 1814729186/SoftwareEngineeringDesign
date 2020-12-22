using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerSaMan : NPCController
{
    // Start is called before the first frame update
    protected override void Start()
    {
            talk = new string[] { "你好，朋友，欢迎来到我们的集镇，这里是朵尔，镇上一贯很清静，你会喜欢这里的。",
                                    "镇上有一个武馆，很久没有招生了，如果你有兴趣，可以去看看。哦，对了，武馆师傅的名字是大漠。",
                                    "镇上有一家商店，老板喜欢卖一些稀奇古怪的东西，价格不低，价格？我可没买过，别问我。",
                                    "你似乎并不健谈，镇上还是一样安静，也罢，享受安静可不是常有的事。",
                                    "又一位旅者离开了，呼…谁会回来呢？"
           };
        random = new System.Random((int)Time.time);
        player = GameObject.Find("Player");
        name = "年迈的萨满：";
    }

}
