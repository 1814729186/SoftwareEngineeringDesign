using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Vector3 Speed;
    public Vector3 EdgeL, EdgeR;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Time.deltaTime);
        if (transform.position.x > EdgeR.x) transform.position = EdgeL;
        if (transform.position.x < EdgeL.x) transform.position = EdgeR;
    }
}
