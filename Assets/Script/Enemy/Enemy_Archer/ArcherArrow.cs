using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 8f;
    [SerializeField] private float defaultLifeTime = 4f;

    private EnemyStats ownerStats;
    private LayerMask playerLayer;
    private Rigidbody2D rb;

    public void Setup(Vector2 direction, float speed, float lifeTime, EnemyStats stats, LayerMask targetLayer)
    {
        rb = GetComponent<Rigidbody2D>();
        ownerStats = stats;
        playerLayer = targetLayer;

        float finalSpeed = speed <= 0 ? defaultSpeed : speed;
        float finalLifeTime = lifeTime <= 0 ? defaultLifeTime : lifeTime;

        if (rb != null)
            rb.velocity = direction.normalized * finalSpeed;

        Destroy(gameObject, finalLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerStats target = collision.GetComponent<PlayerStats>();
            if (target != null && ownerStats != null)
                ownerStats.DoDamage(target);

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
