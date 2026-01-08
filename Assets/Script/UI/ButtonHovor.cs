using UnityEngine;
using UnityEngine.EventSystems; // 必须引用这个

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float hoverScale = 1.1f; // 放大倍数
    public float speed = 10f;       // 缩放速度
    
    [Header("Sound Settings")]
    public AudioClip hoverSound;    // 悬停音效
    public AudioClip clickSound;    // 点击音效

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        // 记录按钮原始大小
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // 平滑过渡到目标大小 (使用 unscaledDeltaTime 以支持暂停菜单)
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * speed);
    }

    // 鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        
        if (hoverSound != null)
        {
            AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
        }
    }

    // 鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
    
    // 鼠标点击
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }
    }
}