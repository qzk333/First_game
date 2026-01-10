using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Signpost : MonoBehaviour
{
    [TextArea(3, 10)]
    public string message = "这里写告示牌的内容...";
    public GameObject interactCue; // 告示牌上方的提示物体
    private bool isPlayerInProximity;

    private void Start()
    {
        if (interactCue != null)
        {
            interactCue.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInProximity && Input.GetKeyDown(KeyCode.W))
        {
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.ShowMessage(message);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isPlayerInProximity = true;
            if (interactCue != null)
            {
                interactCue.SetActive(true);
            }

            // Auto Save
            if (SaveManager.instance != null)
            {
                SaveManager.instance.SaveGame();
            }

            // Show Notification
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.ShowSaveNotification();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isPlayerInProximity = false;
            if (interactCue != null)
            {
                interactCue.SetActive(false);
            }
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.HideMessage();
            }
        }
    }
}
