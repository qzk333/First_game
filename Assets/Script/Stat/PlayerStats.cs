using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    
    [Header("Counter Attack Settings")]
    [Range(0f, 1f)]
    [Tooltip("反击时的伤害减免比例 (0 = 无减免, 1 = 完全格挡)")]
    [SerializeField] private float counterAttackDamageReduction = 1f; // 默认完全格挡
    
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        // 如果在反击状态中，根据减伤比例减少伤害
        if (player.stateMachine.currentstate == player.counterAttack)
        {
            // 如果是完全格挡（减伤比例 = 1）
            if (counterAttackDamageReduction >= 1f)
            {
                Debug.Log("Counter attack! Perfect block!");
                return;
            }
            // 部分减伤
            else if (counterAttackDamageReduction > 0f)
            {
                int reducedDamage = Mathf.RoundToInt(_damage * (1f - counterAttackDamageReduction));
                Debug.Log($"Counter attack! Damage reduced from {_damage} to {reducedDamage}");
                _damage = reducedDamage;
            }
        }
        
        base.TakeDamage(_damage);

        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
