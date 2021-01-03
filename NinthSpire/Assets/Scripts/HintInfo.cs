using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintInfo : MonoBehaviour
{
    [SerializeField]private GameObject Player;
    private bool eclipse;
    private Color initColor;
    private TextMesh meshHandler;
    private void Start()
    {
        meshHandler = GetComponent<TextMesh>();
        GetComponent<MeshRenderer>().sortingOrder = 20;
        initColor = meshHandler.color;
        meshHandler.color = new Color(255f, 255f, 255f, 0f);
        eclipse = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Player.layer)
        {
            if (eclipse)
            {
                meshHandler.color = initColor;
                eclipse = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Player.layer)
        {
            if (!eclipse)
            {
                meshHandler.color = new Color(255f, 255f, 255f, 0f);
                eclipse = true;
            }
        }
    }
}
