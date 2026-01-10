using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContactDamage : MonoBehaviour
{
    private Enemy_Boss boss;

    private void Start()
    {
        boss = GetComponentInParent<Enemy_Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !boss.stats.isDead) // Don't damage if boss is dead
        {
            boss.stats.DoDamage(collision.GetComponent<PlayerStats>());
        }
    }
}
