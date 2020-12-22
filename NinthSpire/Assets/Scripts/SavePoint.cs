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

    [Header("物体类型")]
    public int type;    //0--存档点，1--永恒纹章，2--静谧纹章，3--场景转移
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        cursor = this.transform.Find("Cursor").gameObject;
        cursor.GetComponent<SpriteRenderer>().enabled = false;
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if((Mathf.Abs(player.transform.position.x - this.transform.position.x) + Mathf.Abs(player.transform.position.y - this.transform.position.y))< 2f)
            cursor.GetComponent<SpriteRenderer>().enabled = true;
        else cursor.GetComponent<SpriteRenderer>().enabled = false;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if ( Input.GetKey(KeyCode.J))
            {
                switch (type)
                {
                    case 0:
                        collision.GetComponent<PlayerController>().rebornPosition = this.transform.position;
                        GameObject.Find("Config").GetComponent<Config>().WarningText("存档成功");
                        //Debug.Log("Save");
                        break;
                    case 1:
                        collision.GetComponent<PlayerController>().castAbility = 1;
                        GameObject.Find("Config").GetComponent<Config>().CastAbility = 1;
                        GameObject.Find("Config").GetComponent<Bag>().foreverMedallion = 1;
                        GameObject.Find("Config").GetComponent<Config>().WarningText("已获得永恒纹章");
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        collision.GetComponent<PlayerController>().castAbility = 2;
                        GameObject.Find("Config").GetComponent<Config>().CastAbility = 2;
                        GameObject.Find("Config").GetComponent<Bag>().quietMedallion = 1;
                        GameObject.Find("Config").GetComponent<Config>().WarningText("已获得静谧纹章");
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
