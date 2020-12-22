using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform CameraFixedSpot;    //相机固定位
    public Transform EdgeLT, EdgeLB, EdgeRT, EdgeRB;    //相机边界

    private Camera view;
    private float height, width;
    private AudioSource bgm;

    Vector3 cameraSpot;
    GameObject player;

    private void Start()
    {
        view = GetComponent<Camera>();
        height = view.orthographicSize;
        width = view.aspect * height;
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;
        bgm.Play();

        player = GameObject.Find("Player");
        cameraSpot = new Vector3(0, 0, 0) + CameraFixedSpot.position;
    }

    // Update is called once per frame
    void Update()
    {
        camrea();
        Vector3 tmpPos = Vector3.Lerp(transform.position, new Vector3(cameraSpot.x, cameraSpot.y, transform.position.z), Time.deltaTime * 3);
        
        transform.position = new Vector3(
            tmpPos.x < EdgeLT.position.x + width ? EdgeLT.position.x + width : (tmpPos.x > EdgeRB.position.x - width ? EdgeRB.position.x - width : tmpPos.x),
            tmpPos.y < EdgeRB.position.y + height ? EdgeRB.position.y + height : (tmpPos.y > EdgeLT.position.y - height ? EdgeLT.position.y - height: tmpPos.y),
            tmpPos.z);

        
    }
    public void camrea()
    {
        if (player.GetComponent<Rigidbody2D>().velocity.x == 0 && player.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            if (Input.GetKey(KeyCode.W))//上移
            {
                cameraSpot = CameraFixedSpot.position + new Vector3(0, 3, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                cameraSpot = CameraFixedSpot.position + new Vector3(0, -4, 0);
            }
            else
            {
                cameraSpot = CameraFixedSpot.position;
            }
        }
        else
        {
            cameraSpot = CameraFixedSpot.position;
        }
    }

}
