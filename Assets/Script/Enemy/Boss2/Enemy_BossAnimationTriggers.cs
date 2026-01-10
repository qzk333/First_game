using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossAnimationTriggers : MonoBehaviour
{
    private Enemy_Boss enemy => GetComponentInParent<Enemy_Boss>();

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                PlayerStats targetStats = hit.GetComponent<PlayerStats>();

                // 【核心修改】：判断总次数是否是 3 的倍数
                if (enemy.attackCounter % 3 == 0)
                {
                    // 2 倍伤害
                    float enhancedDamage = enemy.stats.damage.GetValue() * 2;
                    targetStats.TakeDamage(Mathf.RoundToInt(enhancedDamage));

                    // 触发玩家受击效果（包含击退逻辑）
                    player.DamageEffect();
                    Debug.Log($"第 {enemy.attackCounter} 次攻击：骑兵重型冲击！");
                }
                else
                {
                    // 普通伤害
                    enemy.stats.DoDamage(targetStats);
                }
            }
        }
    }

    private void AnimationTrigger() => enemy.AnimationFinishTrigger();
}
