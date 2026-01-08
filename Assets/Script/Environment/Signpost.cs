using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour
{
    [TextArea(3, 10)]
    public string message = "这里写告示牌的内容...";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.ShowMessage(message);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.HideMessage();
            }
        }
    }
}
