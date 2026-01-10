using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : EnemyStats
{
    [Header("Passive Healing")]
    public float healDelay = 5.0f;     // Time without damage before healing starts
    public float healInterval = 1.0f;  // Time between heal ticks
    public int healAmount = 5;         // Amount to heal per tick
    public float healPercentage = 0.05f; // Or heal by percentage of max health

    private float lastDamageTime;
    private float healTimer;

    protected override void Start()
    {
        base.Start();
        lastDamageTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();
        CheckPassiveHealing();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        lastDamageTime = Time.time; // Reset timer on damage
    }

    private void CheckPassiveHealing()
    {
        if (isDead) return;

        if (Time.time > lastDamageTime + healDelay)
        {
            healTimer -= Time.deltaTime;
            if (healTimer < 0)
            {
                healTimer = healInterval;
                IncreaseHealth(healAmount);
            }
        }
    }

    private void IncreaseHealth(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }
    }
}
