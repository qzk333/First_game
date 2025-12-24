using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;    // ÉËº¦
    public Stat agility;     // ÉÁ±Ü
    // public Stat intelligence;    // Ä§·¨ÉËº¦
    public Stat vitality;    // »ØÑª

    [Header("Defensive stats")]
    public Stat maxHealth;    // ÑªÁ¿
    public Stat armor;    // ¿ø¼×·ÀÓù
    public Stat evasion;

    public Stat damage;


    [SerializeField] public int currentHealth;
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

        // ÀýÈç£º×°±¸ÇàÁúµ¶¹¥»÷ + 4
        //damage.AddModifier(4);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()
    {
        // throw new NotImplementedException();
    }


    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

}
