using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRageBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    
    [Header("Rage Bar UI")]
    [SerializeField] private Slider rageSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Optional: Rage Text")]
    [SerializeField] private TextMeshProUGUI rageText;
    [SerializeField] private bool showText = true;
    
    [Header("Visual Settings")]
    [SerializeField] private Color emptyColor = Color.gray;
    [SerializeField] private Color fullColor = new Color(1f, 0.5f, 0f); // Orange/Yellowish for Soul/Rage
    [SerializeField] private bool smoothTransition = true;
    [SerializeField] private float transitionSpeed = 5f;
    
    private float targetRage;

    private void Start()
    {
        // 自动查找 PlayerStats
        if (playerStats == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }
        
        if (rageSlider == null)
            rageSlider = GetComponentInChildren<Slider>();
            
        if (fillImage == null && rageSlider != null)
            fillImage = rageSlider.fillRect.GetComponent<Image>();
        
        // 初始化 Slider 数值
        if (playerStats != null)
        {
            rageSlider.maxValue = playerStats.maxRage;
            rageSlider.value = playerStats.currentRage;
            targetRage = playerStats.currentRage;
            
            // 设置颜色
            if (fillImage != null)
                fillImage.color = fullColor;
        }
    }
    
    private void Update()
    {
        if (playerStats == null || rageSlider == null)
            return;
        
        float currentRage = playerStats.currentRage;
        float maxRage = playerStats.maxRage;
        
        // 确保 MaxValue 正确 (万一升级了怒气上限)
        if (rageSlider.maxValue != maxRage)
            rageSlider.maxValue = maxRage;

        targetRage = currentRage;
        
        // 平滑过渡
        if (smoothTransition)
        {
            rageSlider.value = Mathf.Lerp(rageSlider.value, targetRage, Time.deltaTime * transitionSpeed);
        }
        else
        {
            rageSlider.value = targetRage;
        }
        
        // 更新文字
        if (showText && rageText != null)
        {
            rageText.text = $"{Mathf.RoundToInt(currentRage)} / {Mathf.RoundToInt(maxRage)}";
        }
        
        // 简单的颜色变化 (可选，例如怒气满时发光)
        // 这里暂时保持固定颜色
    }
}
