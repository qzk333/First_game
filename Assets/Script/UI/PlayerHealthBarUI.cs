using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterStats playerStats;
    
    [Header("Health Bar UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Optional: Health Text")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private bool showHealthText = true;
    
    [Header("Health Bar Colors")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color hurtColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    
    [Header("Animation Settings")]
    [SerializeField] private bool smoothTransition = true;
    [SerializeField] private float transitionSpeed = 5f;
    
    private float targetHealth;
    
    private void Start()
    {
        // 如果没有手动设置，尝试自动查找玩家的Stats
        if (playerStats == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<CharacterStats>();
        }
        
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();
            
        if (fillImage == null && healthSlider != null)
            fillImage = healthSlider.fillRect.GetComponent<Image>();
        
        // 初始化血条
        if (playerStats != null)
        {
            float maxHealth = playerStats.maxHealth.GetValue();
            healthSlider.maxValue = maxHealth;
            healthSlider.value = playerStats.currentHealth;
            targetHealth = playerStats.currentHealth;
        }
    }
    
    private void Update()
    {
        if (playerStats == null || healthSlider == null)
            return;
        
        float maxHealth = playerStats.maxHealth.GetValue();
        float currentHealth = playerStats.currentHealth;
        
        // 更新目标血量
        targetHealth = currentHealth;
        
        // 平滑过渡或立即更新
        if (smoothTransition)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealth, Time.deltaTime * transitionSpeed);
        }
        else
        {
            healthSlider.value = currentHealth;
        }
        
        // 计算血量百分比
        float healthPercent = currentHealth / maxHealth;
        
        // 更新颜色
        UpdateHealthColor(healthPercent);
        
        // 更新文字显示
        if (showHealthText && healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {Mathf.RoundToInt(maxHealth)}";
        }
    }
    
    private void UpdateHealthColor(float healthPercent)
    {
        if (fillImage == null)
            return;
        
        if (healthPercent > 0.6f)
            fillImage.color = healthyColor;
        else if (healthPercent > 0.3f)
            fillImage.color = hurtColor;
        else
            fillImage.color = criticalColor;
    }
    
    // 可以从外部调用来设置玩家Stats引用
    public void SetPlayerStats(CharacterStats stats)
    {
        playerStats = stats;
    }
}
