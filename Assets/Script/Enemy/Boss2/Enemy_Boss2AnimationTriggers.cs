using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss2AnimationTriggers : MonoBehaviour
{
    private Enemy_Boss2 enemy => GetComponentInParent<Enemy_Boss2>();

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
                if (enemy.stateMachine.currentState == enemy.chargeState || (enemy.attackCounter > 0 && enemy.attackCounter % 3 == 0))
                {
                    float enhancedDamage = enemy.stats.damage.GetValue() * 2f; // 2倍伤害
                    targetStats.TakeDamage(Mathf.RoundToInt(enhancedDamage));
                    player.DamageEffect(); // 触发击退
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
