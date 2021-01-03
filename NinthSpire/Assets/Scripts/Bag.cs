using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bag : MonoBehaviour
{
    /// <summary>
    /// 背包系统实现,存放于Config物体
    /// 为实现简单，采用静态背包系统，固定位置存放固定物体
    /// </summary>
    // Start is called before the first frame update
    [Header("基本物品")]
    [SerializeField] public int coin;   //硬币数量
    [SerializeField] public int healMedicineNum;
    [SerializeField] public int foreverMedallion;
    [SerializeField] public int quietMedallion;
    GameObject bag;
    bool bagIsOn;
    public void Start()
    {
        bagIsOn = false;
        transform.Find("Canvas").Find("WarningText").GetComponent<Text>().enabled = false;
        foreverMedallion = 0;
        quietMedallion = 0;//纹章数量
        healMedicineNum = 10;
        bag = this.transform.Find("Bag").gameObject; //找到背包游戏对象
        bag.GetComponent<UnityEngine.Canvas>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        ArrangeBag();
        if (SceneManager.GetActiveScene().name != "StartScene" && GetComponent<Config>().pause == false && Input.GetKeyDown(KeyCode.B))
        {
            if (bagIsOn)//关闭背包
            {
                CloseBag();
                bagIsOn = false;
            }
            else//打开背包
            {
                OpenBag();
                bagIsOn = true;
            }
        }
        //使用药水
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            useHealMedicine();
        }



    }
    public void useHealMedicine()
    {
        //药水数量为零，提示药瓶不足
        if (GameObject.Find("Player") != null)
        {
            if (healMedicineNum <= 0)
            {
                transform.Find("Canvas").Find("WarningText").GetComponent<Text>().text = "药品数量不足";
            }
            else if (GameObject.Find("Player").GetComponent<PlayerController>().HP >= 3)
            {
                transform.Find("Canvas").Find("WarningText").GetComponent<Text>().text = "当前血量已满";
            }
            else
            {
                //使用药品
                healMedicineNum--;
                GameObject.Find("Player").GetComponent<PlayerController>().HP = 3;
                GetComponent<Config>().Health = 3;
                transform.Find("Canvas").Find("WarningText").GetComponent<Text>().text = "使用药品成功";
            }
            transform.Find("Canvas").Find("WarningText").GetComponent<Text>().enabled = true;
            StartCoroutine(WarningTextFadeIEnu());
        }

    }
    IEnumerator WarningTextFadeIEnu()
    {
        yield return new WaitForSeconds(1.0f);
        transform.Find("Canvas").Find("WarningText").GetComponent<Text>().enabled = false;
    }

    //背包UI管理
    void ArrangeBag()
    {
        //更新背包参数值
        transform.Find("Canvas").Find("HealMedicine").Find("Num").GetComponent<Text>().text = healMedicineNum.ToString();
        Transform wrapper = bag.transform.Find("wrapper");
        wrapper.Find("HealMedicine").Find("Num").GetComponent<Text>().text = healMedicineNum.ToString();
        if (foreverMedallion == 0)
            wrapper.Find("foreverMedallion").GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        else
            wrapper.Find("foreverMedallion").GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        if (quietMedallion == 0)
            wrapper.Find("quietMedallion").GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        else
            wrapper.Find("quietMedallion").GetComponent<Image>().color = new Color(1, 1, 1, 1f);

    }


    //打开与关闭背包
    void OpenBag()
    {
        bag.GetComponent<UnityEngine.Canvas>().enabled = true;  //打开背包
        Time.timeScale = 0.0f;  //游戏暂停


    }
    void CloseBag()
    {
        bag.GetComponent<UnityEngine.Canvas>().enabled = false;  //打开背包
        Time.timeScale = 1.0f;  //游戏开始
    }


}
