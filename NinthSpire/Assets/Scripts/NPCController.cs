using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public string[] talk;
    protected System.Random random;
    protected GameObject player;
    protected string name;
    protected virtual void Start()
    {
        talk = new string[] { "哎哎，没什么事的话就别来烦我，圣堂骑士可不会解答你的生活问题。",
                                    "别看我现在这样，我可是传说中的圣堂骑士，看见我的长枪了吗，那可是“午夜寒锋”，无数敌人倒在她的刃下。",
                                    "想知道我的实力，算了吧，在你能处理好成群结队的飞虫之前，我可不会告诉你什么叫“旋风斩”。",
                                    "从外面来的？那你看见那台诡异的电梯了吧，那不是什么好东西，不如攀墙来的实在。",
                                    "我的印章？我不需要印章，那是用来祭奠死者的，快走快走，别来烦我。",
                                    "骑士守则第一条：永远不要相信理所应当的事情。",
                                    "骑士守则第二条：待在真相的背面。",
                                    "不错的小玩意，这东西可是以前圣堂骑士才有资格拿到的东西，不过最近到处都能捡到了…哎…荣誉的抛弃时刻…",
                                    "结界之战杀死了太多的骑士，它们的荣誉也长存此地了。",
                                    "玷污的印章，它本不该如此璀璨，正如你所见，光辉亮丽的东西总是会遮盖灰暗的真实，所以我喜欢待在暗处…莫塔路…你背叛了吗…"
           };
        random = new System.Random((int)Time.time);
        player = GameObject.Find("Player");
        name = "杰特：";
    }

    protected virtual void Update()
    {
        if(calulateDis(player.transform.position,this.transform.position) < 1f)
        {
            Debug.Log("talk");
            if (Input.GetKeyDown(KeyCode.W))
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
            }
        }
    }
    protected virtual float calulateDis(Vector2 v1,Vector2 v2)
    {
        float res = 0 ;
        res = Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y-v2.y);
        return res;
    }




}
