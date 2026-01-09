using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Trigger Settings")]
    public bool destroyAfterTrigger = true; // 触发后是否销毁物体

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            UnlockSkill(player);
            
            if (UI_MessageOverlay.instance != null)
            {
                UI_MessageOverlay.instance.ShowMessage(unlockMessage);
                // 如果需要延时关闭消息，可以在这里处理，或者让 UI_MessageOverlay 自动处理
                // 目前 UI_MessageOverlay 需要手动 HideMessage，或者可以增加一个 ShowMessageWithDuration
                // 这里暂时假设玩家走过去后会一直显示直到离开（如果不销毁）或者显示一段时间
            }

            if (destroyAfterTrigger)
            {
                // 稍微延迟销毁以免消息瞬间消失（如果UI依赖于这个物体，但UI是单例，所以没关系）
                // 但是如果触发器作为路标存在，可能不想销毁 visual，只是禁用 collider
                // 这里简单起见，禁用 Collider 或 脚本
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
            }
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
