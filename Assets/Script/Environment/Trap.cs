using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            player.stats.TakeDamage(damage);
            player.RespawnAtSafePosition();
        }
    }
}
