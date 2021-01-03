using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCControllerSiLai : NPCController
{
    bool shopUIOpen;
   protected override void Start()
    {
            talk = new string[] { "武学参考指南，可以卖多少钱…不行…不能卖，这可是我的藏书。",
                                    "光萤灯笼，幽微而不喧闹的亮度，反而是黑暗中唯一不会熄灭的光，但是黑暗的村庄还有谁会去么…",
                                    "喔，你居然想买下我的全部家当，我很赞许你的做法，不过要等一等了，毕竟进货渠道可只有我自己。",
           };
        random = new System.Random((int)Time.time);
        player = GameObject.Find("Player");
        name = "斯莱：";
        shopUIOpen = false;
    }

    protected override void Update()
    {
        if (calulateDis(player.transform.position, this.transform.position) < 1f)
        {
            if (Input.GetKeyDown(KeyCode.W)&&(!shopUIOpen))
            {
                int index = random.Next(talk.Length);//随机播放一段话
                transform.Find("Canvas/Image/Text").GetComponent<Text>().text = name + talk[index];
                transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled = true;

            }

        }
        if (transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled = false;
                //打开ShopUI界面，并设置timeScale
                OpenShopUI(true);
            }
        }
    }
    public void buyHealMedicine()  //购买回复药品
    {
        GameObject config = GameObject.Find("Config");
        if (config.GetComponent<Bag>().coin >= 10)
        {
            config.GetComponent<Bag>().coin -= 10;
            config.GetComponent<Config>().coinNum -= 10;
            config.GetComponent<Bag>().healMedicineNum++;
            config.GetComponent<Config>().WarningText("购买成功，当前药品数量为：" + config.GetComponent<Bag>().healMedicineNum.ToString());
        }
        else
        {
            config.GetComponent<Config>().WarningText("购买失败，货币不足");
        }
    }
    public void OpenShopUI(bool state)//为true表示打开，为false表示关闭
    {
        this.transform.Find("Shop").GetComponent<UnityEngine.Canvas>().enabled = state;
        shopUIOpen = state;
        Time.timeScale = state ? 0.0f : 1.0f;   //设置暂停
    }

}
