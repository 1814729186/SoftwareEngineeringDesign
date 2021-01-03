using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer:MaZhongping
/// Describe：存档点使用脚本，靠近存档点并按j键即可完成检测，更新重生点
/// </summary>
public class SavePoint : MonoBehaviour
{
    GameObject cursor;
    GameObject config;

    [Header("物体类型")]
    public int type;    //0--存档点，1--永恒纹章，2--静谧纹章，3--场景转移
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        cursor = this.transform.Find("Cursor").gameObject;
        cursor.GetComponent<SpriteRenderer>().enabled = false;
        player = GameObject.Find("Player");
        config = GameObject.Find("Config");
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(player.transform.position.x - this.transform.position.x) < 0.7f)
            cursor.GetComponent<SpriteRenderer>().enabled = true;
        else cursor.GetComponent<SpriteRenderer>().enabled = false;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Input.GetKey(KeyCode.J))
            {
                switch (type)
                {
                    case 0:
                        player.GetComponent<PlayerController>().rebornPosition = this.transform.position;
                        config.GetComponent<Config>().WarningText("存档成功");
                        //Debug.Log("Save");
                        break;
                    case 1:
                        player.GetComponent<PlayerController>().castAbility = 1;
                        config.GetComponent<Config>().CastAbility = 1;
                        config.GetComponent<Bag>().foreverMedallion = 1;
                        config.GetComponent<Config>().WarningText("已获得永恒纹章");
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        player.GetComponent<PlayerController>().castAbility = 2;
                        config.GetComponent<Config>().Level += 1;
                        config.GetComponent<Config>().CastAbility = 2;
                        config.GetComponent<Bag>().quietMedallion = 1;
                        config.GetComponent<Config>().WarningText("已获得静谧纹章");
                        Destroy(this.gameObject);
                        break;
                    case 3:
                        GetComponent<LevelChange>().TransferScene();
                        break;
                }


            }

        }
    }
}
