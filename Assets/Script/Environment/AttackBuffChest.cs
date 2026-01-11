using System.Collections;
using UnityEngine;
using TMPro;

// Attach to chest to grant an attack bonus once.
public class AttackBuffChest : MonoBehaviour
{
    [Header("Buff Settings")]
    [SerializeField] private int attackBonus = 5;
    [SerializeField] private string message = "攻击力提升！";
    [SerializeField] private float messageDuration = 2f;

    [Header("Local UI (optional)")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Trigger Settings")]
    [SerializeField] private bool destroyAfterUse = false;   // 是否销毁宝箱
    [SerializeField] private bool disableColliderAfterUse = true; // 不销毁则关闭碰撞

    private bool used;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) return;

        Player player = collision.GetComponent<Player>();
        if (player != null && player.stats != null && player.stats.damage != null)
        {
            player.stats.damage.AddModifier(attackBonus);
            used = true;
            ShowMessage();
            StartCoroutine(HandlePostUse());
        }
    }

    private void ShowMessage()
    {
        if (messagePanel == null || messageText == null)
            return;

        messageText.text = message;
        messagePanel.SetActive(true);
        if (messageDuration > 0)
            StartCoroutine(HideMessageAfterDelay(messageDuration));
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    private IEnumerator HandlePostUse()
    {
        // Wait for message duration before cleanup
        if (messageDuration > 0)
            yield return new WaitForSeconds(messageDuration);

        if (messagePanel != null)
            messagePanel.SetActive(false);

        if (destroyAfterUse)
        {
            Destroy(gameObject);
            yield break;
        }

        if (disableColliderAfterUse)
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        // 可选：播放开箱动画/更换外观，可在这里扩展
    }
}
