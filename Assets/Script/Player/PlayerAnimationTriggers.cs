using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                
                // 先加怒气，再造成伤害（防止打死后脚本失效或其他问题导致还没加怒气）
                player.stats.IncreaseRage(1);
                
                player.stats.DoDamage(_target);
            }
        }

    }
    
    // 反击攻击触发器，在成功反击动画中调用
    public void CounterAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                
                // 反击命中也增加怒气 (先加再伤)
                player.stats.IncreaseRage(1);
                
                player.stats.DoDamage(_target);
            }
        }
    }

    // 远程攻击触发器
    public void RangedAttackTrigger()
    {
        if (player.swordWavePrefab != null)
        {
            // 生成刀波
            // 使用 attackCheck 而不是 transform.position，防止生成在脚底撞到地面瞬间消失
            // 同时也让刀波出现在角色前方，更符合视觉
            GameObject newSwordWave = Instantiate(player.swordWavePrefab, player.attackCheck.position, Quaternion.identity);
            
            // 获取控制脚本
            SwordWaveController controller = newSwordWave.GetComponent<SwordWaveController>();
            
            if (controller != null)
            {
                // 计算实际伤害：(基础伤害 + 力量) * 倍率
                // 我们直接从 Player 的 CharacterStats 组件读取数值
                int baseDamage = 10;
                
                if (player.stats != null)
                {
                    baseDamage = player.stats.damage.GetValue() + player.stats.strength.GetValue();
                }
                
                float finalDamage = baseDamage * player.rangedDamageMultiplier;
                
                controller.Setup(finalDamage, player.facingDir);
            }
        }
        else
        {
            Debug.LogWarning("Sword Wave Prefab not assigned on Player!");
        }
    }
}
