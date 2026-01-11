using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArcherAnimationTriggers : MonoBehaviour
{
    private Enemy_Archer enemy => GetComponentInParent<Enemy_Archer>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        enemy.FireArrow();
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

    public void DespawnEnemy()
    {
        Destroy(enemy.gameObject);
    }
}
