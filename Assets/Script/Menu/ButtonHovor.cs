using UnityEngine;
using UnityEngine.EventSystems; // 必须引用这个

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float hoverScale = 1.1f; // 放大倍数
    public float speed = 10f;       // 缩放速度

    private Vector3 targetScale;

    void Start()
    {
        // 记录按钮原始大小
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // 平滑过渡到目标大小
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    // 鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        // 如果有音效，可以在这里播放，例如：
        // AudioSource.PlayClipAtPoint(hoverSound, transform.position);
    }

    // 鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}