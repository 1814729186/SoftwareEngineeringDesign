using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音效控制器
public class AudioManger
{

    private static AudioManger instance;    //音效控制器实例
    public static AudioManger Instance      //实例化音效控制器并返回音效控制器句柄
    {
        get
        {
            if (instance == null)       //音效控制器无实例时创建新实例
            {
                instance = new AudioManger();
            }
            return instance;            //返回句柄
        }
    }
    private string path = "Audio/";     //音效资源目录为“Resources/Audio/”
    public Dictionary<string, AudioClip> clips;
    public Dictionary<int, GameObject> sourseObjs;

    //构造函数
    public AudioManger()
    {
        clips = new Dictionary<string, AudioClip>();        //创建音效片段字典
        sourseObjs = new Dictionary<int, GameObject>();     //创建音效句柄字典
    }

    //按名播放音效
    public void PlayAudio(string clipName)
    {
        //获取音效片段句柄
        AudioClip tempPlayClip = CheckPlayClip(clipName);
        //在主摄影机位置播放音效
        AudioSource.PlayClipAtPoint(tempPlayClip, Camera.main.transform.position);
    }

    //按名在特定位置播放音效
    public void PlayAudio(string clipName, Vector3 playPoint)
    {
        //获取音效片段句柄
        AudioClip tempPlayClip = CheckPlayClip(clipName);
        //在playPoint位置播放音效
        AudioSource.PlayClipAtPoint(tempPlayClip, playPoint);
    }

    //将音效游戏对象创建为子物体，跟随父物体，并返回音效源句柄
    public AudioSource PlayAudio(string clipName, Transform parent, int index)
    {
        if (sourseObjs.ContainsKey(index))
        {
            //销毁已存在的同键值实例
            sourseObjs.Remove(index);
        }
        //获取音效片段句柄
        AudioClip tempPlayClip = CheckPlayClip(clipName);
        //新建游戏对象并添加音效源AudioSource组件
        GameObject tempObj = new GameObject();
        AudioSource source = tempObj.AddComponent<AudioSource>();
        //音效游戏对象挂载到父物体并对齐到本地坐标
        tempObj.transform.SetParent(parent);
        tempObj.transform.localPosition = Vector3.zero;
        //设置音效源AudioSource组件参数，使其播放当前音效
        source.volume = GameObject.Find("Config").GetComponent<Config>().musicVolum;
        source.clip = tempPlayClip;
        source.loop = true;
        source.Play();
        //音效游戏对象加入字典
        sourseObjs.Add(index, tempObj);
        return source;
    }

    //销毁音效游戏对象
    public void DeleteClipObj(int index)
    {
        if (sourseObjs.ContainsKey(index))
        {
            GameObject tempObj = sourseObjs[index]; //获取音效游戏对象句柄
            sourseObjs.Remove(index);               //从音效游戏对象字典移除
            GameObject.Destroy(tempObj);            //销毁音效游戏对象
        }
    }

    //加载音效片段
    private AudioClip LoadClip(string path)
    {
        AudioClip tempClip = Resources.Load<AudioClip>(path);   //加载音效片段
        if (tempClip != null)
        {
            return tempClip;
        }
        else
        {
            Debug.Log("读取音频文件失败");
            return null;
        }
    }

    //获取音效片段句柄
    private AudioClip CheckPlayClip(string clipName)
    {
        AudioClip tempPlayClip = null;
        if (!clips.ContainsKey(clipName))
        {
            //如果这是一个未引用的音效
            string tempPath = path + clipName;
            //载入并添加到字典
            tempPlayClip = LoadClip(tempPath);
            clips.Add(clipName, tempPlayClip);
        }
        else
        {
            //有引用的音效直接返回
            tempPlayClip = clips[clipName];
        }
        return tempPlayClip;
    }
}
