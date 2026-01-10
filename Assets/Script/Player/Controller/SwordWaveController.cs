using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWaveController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    
    private float damage;
    private int facingDir;
    private Rigidbody2D rb;

    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 几秒后自动销毁，防止无限飞
        Destroy(gameObject, lifeTime);
    }

    public void Setup(float _damage, int _dir)
    {
        damage = _damage;
        facingDir = _dir;
        
        // 初始速度
        rb.velocity = new Vector2(speed * facingDir, 0);

        // 调整朝向 (如果刀波图片默认朝右)
        if (facingDir < 0)
            transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        // 如果击中敌人
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats targetStats = collision.GetComponent<EnemyStats>();
            if (targetStats != null)
            {
                // 计算最终伤害 (简单地减去护甲，模拟 CharacterStats 逻辑)
                int finalIncomingDamage = (int)damage;
                
                if (targetStats.armor != null)
                {
                    finalIncomingDamage -= targetStats.armor.GetValue();
                }
                
                finalIncomingDamage = Mathf.Clamp(finalIncomingDamage, 1, int.MaxValue); // 至少造成1点伤害

                targetStats.TakeDamage(finalIncomingDamage);
                
                hasHit = true;
                Destroy(gameObject); // 或者播放击中特效
            }
        }
        // 如果撞墙 (假设墙层级叫 "Ground")
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasHit = true;
            Destroy(gameObject); // 撞墙消失
        }
    }
}
