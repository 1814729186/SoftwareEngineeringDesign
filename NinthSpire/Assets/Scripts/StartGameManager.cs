using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    public string LevelTarget;
    public void StartGame()
    {
        GameObject.Find("Config").transform.Find("Canvas").GetComponent<UnityEngine.Canvas>().enabled = true;
        //初始化config中相关数据
        GameObject.Find("Config").GetComponent<Config>().Initial();
        GameObject.Find("Config").GetComponent<Bag>().Start();
        SceneManager.LoadScene(LevelTarget);
        
    }
    
    public void changeUI(int UI)    //根据点击，改变UI显示,0--MainUI，1--ConfigUI，2--DeveloperUI
    {
        if(UI == 0)
        {
            transform.Find("MainUI").GetComponent<UnityEngine.Canvas>().enabled = true;
            transform.Find("ConfigUI").GetComponent<UnityEngine.Canvas>().enabled = false;
            transform.Find("DeveloperUI").GetComponent<UnityEngine.Canvas>().enabled = false;
        }
        else if (UI == 1)
        {
            transform.Find("MainUI").GetComponent<UnityEngine.Canvas>().enabled = false;
            transform.Find("ConfigUI").GetComponent<UnityEngine.Canvas>().enabled = true;
            transform.Find("DeveloperUI").GetComponent<UnityEngine.Canvas>().enabled = false;
        }else if(UI == 2)
        {
            transform.Find("MainUI").GetComponent<UnityEngine.Canvas>().enabled = false;
            transform.Find("ConfigUI").GetComponent<UnityEngine.Canvas>().enabled = false;
            transform.Find("DeveloperUI").GetComponent<UnityEngine.Canvas>().enabled = true;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void changeMusic(bool isOn)
    {
        GameObject.Find("Config").GetComponent<Config>().musicOn = isOn;
    }
    public void MusicSlidder()
    {
        GameObject.Find("Config").GetComponent<Config>().musicVolum = transform.Find("ConfigUI").Find("Slider").GetComponent<Slider>().value;
    }

}
