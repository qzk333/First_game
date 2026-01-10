using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MessageOverlay : MonoBehaviour
{
    public static UI_MessageOverlay instance;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject messageContainer; // 可选：用于控制整个面板的显示/隐藏

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        // 初始隐藏
        if(messageText != null)
            messageText.alpha = 0;
            
        if(messageContainer != null)
            messageContainer.SetActive(false);
    }

    public void ShowMessage(string _message)
    {
        if (messageText == null) return;

        messageText.text = _message;
        
        if (messageContainer != null)
            messageContainer.SetActive(true);

        // 淡入效果 (简单实现，也可以用协程)
        StopAllCoroutines();
        StartCoroutine(FadeText(1));
    }

    public void HideMessage()
    {
        if (messageText == null) return;

        // 淡出效果
        StopAllCoroutines();
        StartCoroutine(FadeText(0));
    }

    private IEnumerator FadeText(float _targetAlpha)
    {
        float currentAlpha = messageText.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(currentAlpha, _targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        messageText.alpha = _targetAlpha;

        // 如果完全透明，可以关闭容器节省性能
        if (_targetAlpha == 0 && messageContainer != null)
            messageContainer.SetActive(false);
    }
}
