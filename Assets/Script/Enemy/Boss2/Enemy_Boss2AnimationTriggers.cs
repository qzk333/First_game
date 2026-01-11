using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss2AnimationTriggers : MonoBehaviour
{
    private Enemy_Boss2 enemy => GetComponentInParent<Enemy_Boss2>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            PlayerStats target = hit.GetComponent<PlayerStats>();
            if (target != null)
                enemy.stats.DoDamage(target);
        }
    }

    // Allow dash damage to be driven by an animation event if desired.
    public void DashDamageTrigger()
    {
        enemy.TryDealDashDamage();
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

    public void DespawnEnemy()
    {
        Destroy(enemy.gameObject);
    }
}
