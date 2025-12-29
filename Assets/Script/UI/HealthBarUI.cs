using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private CharacterStats myStats;
    
    [Header("Health Bar Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Health Bar Colors")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color hurtColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    
    [Header("Settings")]
    [SerializeField] private bool hideWhenFull = false;
    [SerializeField] private bool faceCamera = true;
    
    private Camera mainCamera;
    
    private void Start()
    {
        myStats = GetComponentInParent<CharacterStats>();
        mainCamera = Camera.main;
        
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();
            
        if (fillImage == null && healthSlider != null)
            fillImage = healthSlider.fillRect.GetComponent<Image>();
        
        UpdateHealthBar();
    }
    
    private void Update()
    {
        UpdateHealthBar();
        
        // 让血条始终面向摄像机
        if (faceCamera && mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
    
    public void UpdateHealthBar()
    {
        if (myStats == null || healthSlider == null)
            return;
        
        float maxHealth = myStats.maxHealth.GetValue();
        float currentHealth = myStats.currentHealth;
        
        // 更新血条填充值
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        
        // 计算血量百分比
        float healthPercent = currentHealth / maxHealth;
        
        // 根据血量百分比改变颜色
        if (fillImage != null)
        {
            if (healthPercent > 0.6f)
                fillImage.color = healthyColor;
            else if (healthPercent > 0.3f)
                fillImage.color = hurtColor;
            else
                fillImage.color = criticalColor;
        }
        
        // 血满时隐藏血条
        if (hideWhenFull)
        {
            gameObject.SetActive(healthPercent < 1f);
        }
    }
}
