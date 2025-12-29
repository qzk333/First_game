using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MinionAnimationTriggers : MonoBehaviour
{
    private Enemy_Minion enemy => GetComponentInParent<Enemy_Minion>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

}
