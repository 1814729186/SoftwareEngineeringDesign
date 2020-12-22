using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipseMask : MonoBehaviour
{
    public GameObject Player;
    private Vector3 size;
    private float edgeXL, edgeXR, edgeYB, edgeYU;
    private Vector3 initSpot;
    private bool eclipse;
    private void Start()
    {
        size = transform.GetComponent<Renderer>().bounds.size / 2;
        initSpot = this.transform.position;
        edgeXL = initSpot.x - size.x;
        edgeXR = initSpot.x + size.x;
        edgeYB = initSpot.y - size.y;
        edgeYU = initSpot.y + size.y;
        eclipse = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float tmpX = Player.transform.position.x;
        float tmpY = Player.transform.position.y;
        if (tmpX < edgeXR && tmpX > edgeXL && tmpY < edgeYU && tmpY > edgeYB)
        {
            if (!eclipse)
            {
                transform.position = new Vector3(0, 1000, 0);
                eclipse = true;
            }
        }
        else
        {
            transform.position = initSpot;
            eclipse = false;
        }
    }
}
