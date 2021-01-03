using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Programmer：MaZhongping
/// Describe：酸液控制脚本，飞向主角，碰撞到主角后播放销毁动画
/// </summary>
public class Acid : MonoBehaviour
{
    GameObject player;
    [Header("Acid参数")]
    [SerializeField] private float speed = 0.1f;   //酸液飞行速度
    [SerializeField] private float destroyTime = 10.0f; //发出后销毁的时间
    [SerializeField] private float power;   //酸液攻击力
    void Start()
    {
        StartCoroutine(DestroyIEmu());  //调用协程，准备销毁
    }
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0f, 0f);
    }

    IEnumerator DestroyIEmu()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);  //销毁自己
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().beAttack(this.transform, power);
            Destroy(this.gameObject);
        }
    }
}
