using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Config : MonoBehaviour
{
    public Vector3 InitPosition;
    public Vector3 InitRotation;
    public int Health = 3;
    //public int Soul;
    public int coinNum;
    public bool musicOn = true;
    public float musicVolum = 1f;
    public int CastAbility;
    public int Level;
    public bool pause; //是否打开暂停界面

    public void Initial()   //Config初始化
    {
        CastAbility = 0;
        Level = 0;
        Health = 3;
        coinNum = 20;
        GetComponent<Bag>().coin = this.coinNum;
        pause = false;
    }
    private void Start()
    {
        //CastAbility = 0;
        transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled = false;
        musicOn = true;
        Object.DontDestroyOnLoad(this.gameObject);
        Object.DontDestroyOnLoad(transform.Find("Canvas"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("HealPoint1"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("HealPoint2"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("HealPoint3"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("coin"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("coin").Find("coinNum"));
        Object.DontDestroyOnLoad(transform.Find("Canvas").Find("WarningText"));
        Object.DontDestroyOnLoad(transform.Find("PauseUI"));
        Object.DontDestroyOnLoad(transform.Find("PauseUI").Find("Image"));

        Health = 3;
        transform.Find("Canvas").Find("HealPoint1").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        transform.Find("Canvas").Find("HealPoint2").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        transform.Find("Canvas").Find("HealPoint3").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        transform.Find("Canvas").Find("coin").transform.Find("coinNum").GetComponent<Text>().text = coinNum.ToString() ;

        GetComponent<Bag>().coin = this.coinNum;
        pause = false;
    }
    private void Update()
    {
        GetComponent<Bag>().coin = this.coinNum;

        transform.Find("Canvas").Find("coin").transform.Find("coinNum").GetComponent<Text>().text = coinNum.ToString();
        if (Health == 3)
        {
            transform.Find("Canvas").Find("HealPoint1").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.Find("Canvas").Find("HealPoint2").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.Find("Canvas").Find("HealPoint3").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        else if(Health == 2)
        {
            transform.Find("Canvas").Find("HealPoint1").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.Find("Canvas").Find("HealPoint2").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.Find("Canvas").Find("HealPoint3").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }else if(Health == 1)
        {
            transform.Find("Canvas").Find("HealPoint1").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.Find("Canvas").Find("HealPoint2").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            transform.Find("Canvas").Find("HealPoint3").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }else if(Health == 0)
        {
            transform.Find("Canvas").Find("HealPoint1").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            transform.Find("Canvas").Find("HealPoint2").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            transform.Find("Canvas").Find("HealPoint3").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
        GameObject.Find("Main Camera").GetComponent<AudioSource>().volume = musicVolum;
        if(musicOn == false) GameObject.Find("Main Camera").GetComponent<AudioSource>().volume = 0.0f;
        if (SceneManager.GetActiveScene().name !=  "StartScene" && Input.GetKeyDown(KeyCode.Escape))   //按下escape，打开暂停界面
        {
            if(pause == false)
            {
                changeUI(1);    //打开pause
            }
            else
            {
                changeUI(0);    //关闭pause
            }
        }
    }
    public void WarningText(string warningStr)
    {
        transform.Find("Canvas").Find("WarningText").GetComponent<Text>().text = warningStr;
        transform.Find("Canvas").Find("WarningText").GetComponent<Text>().enabled = true;
        StartCoroutine(WarningTextFadeIEnu());
    }
    IEnumerator WarningTextFadeIEnu()
    {
        yield return new WaitForSeconds(1f);
        transform.Find("Canvas").Find("WarningText").GetComponent<Text>().enabled = false;
    }
    //暂停与切换UI界面
    public void changeUI(int state) //0表示关闭所有UI，1表示打开PauseUI，2表示打开HelpUI
    {
        GameObject pauseUI = transform.Find("PauseUI").gameObject;
        GameObject HelpUI = transform.Find("HelpUI").gameObject;
        if (state == 0)
        {
            pauseUI.GetComponent<UnityEngine.Canvas>().enabled = false;
            HelpUI.GetComponent<UnityEngine.Canvas>().enabled = false;
            Time.timeScale = 1.0f; //解除暂停
            pause = false;
        }
        if (state == 1)
        {
            pauseUI.GetComponent<UnityEngine.Canvas>().enabled = true;
            HelpUI.GetComponent<UnityEngine.Canvas>().enabled = false;
            pause = true;
            Time.timeScale = 0.0f;  //暂停
        }
        if(state == 2)
        {
            pauseUI.GetComponent<UnityEngine.Canvas>().enabled = false;
            HelpUI.GetComponent<UnityEngine.Canvas>().enabled = true;
        }
    }

    public void changeScene(string sceneTarget)  //更改游戏场景
    {
        //设置场景显示
        changeUI(0);    //关闭显示
        transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled = false;    //关闭主界面显示
        SceneManager.LoadScene(sceneTarget);    //返回主界面
        if (sceneTarget == "StartScene") Destroy(this.gameObject);
    }
}
