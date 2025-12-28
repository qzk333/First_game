using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        // 只有在敌人未死亡时才播放受击特效
        if (!isDead)
            enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
