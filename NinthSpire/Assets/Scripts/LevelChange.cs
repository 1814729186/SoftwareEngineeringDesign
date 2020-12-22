using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public Vector3 TargetPosition;
    public Vector3 TargetRotation;
    public string LevelTarget;
    public LayerMask WhatIsPlayer;
    private Config config;

    private void Start()
    {
        config = GameObject.Find("Config").GetComponent<Config>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((WhatIsPlayer.value >> collision.gameObject.layer & 1) != 0)
        {
            config.InitPosition = TargetPosition;
            config.InitRotation = TargetRotation;
            SceneManager.LoadScene(LevelTarget);
        }
    }

    public void TransferScene()
    {
        config.InitPosition = TargetPosition;
        config.InitRotation = TargetRotation;
        SceneManager.LoadScene(LevelTarget);
    }
}
