using System.Collections;
using UnityEngine;
using TMPro;

public enum SkillType
{
    Dash,
    RangedAttack
}

public class SkillUnlockTrigger : MonoBehaviour
{
    [Header("Skill Settings")]
    public SkillType skillToUnlock;
    public string unlockMessage;
    [SerializeField] private float messageDuration = 2f;

    [Header("Local UI (optional)")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Trigger Settings")]
    public bool destroyAfterTrigger = true; // 触发后是否销毁物体
    public bool disableColliderOnly = false; // 如果不销毁，仅关闭触发器
    [SerializeField] private bool requireTagMatch = false;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (requireTagMatch && !string.IsNullOrEmpty(playerTag) && !collision.CompareTag(playerTag))
            return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            UnlockSkill(player);
            ShowLocalMessage();

            if (destroyAfterTrigger)
            {
                if (disableColliderOnly)
                {
                    Collider2D col = GetComponent<Collider2D>();
                    if (col != null) col.enabled = false;
                    this.enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void ShowLocalMessage()
    {
        if (messagePanel != null && messageText != null)
        {
            string msg = string.IsNullOrEmpty(unlockMessage) ? GetDefaultMessage() : unlockMessage;
            messageText.text = msg;
            messagePanel.SetActive(true);
            if (messageDuration > 0)
                StartCoroutine(HideMessageAfterDelay(messageDuration));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    private string GetDefaultMessage()
    {
        switch (skillToUnlock)
        {
            case SkillType.Dash:
                return "获得冲刺技能";
            case SkillType.RangedAttack:
                return "获得远程攻击技能";
            default:
                return "技能已解锁";
        }
    }

    private void UnlockSkill(Player _player)
    {
        switch (skillToUnlock)
        {
            case SkillType.Dash:
                _player.unlockDash = true;
                Debug.Log("Dash Unlocked!");
                break;
            case SkillType.RangedAttack:
                _player.unlockRangedAttack = true;
                Debug.Log("Ranged Attack Unlocked!");
                break;
        }
    }
}
