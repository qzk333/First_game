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

    private Coroutine fadeCoroutine;

    public void ShowMessage(string _message)
    {
        if (messageText == null) return;
        messageText.text = _message;
        
        if (messageContainer != null)
            messageContainer.SetActive(true);

        // Manage specific coroutine
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeText(1));
    }

    public void HideMessage()
    {
        if (messageText == null) return;

        // Manage specific coroutine
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeText(0));
    }

    private IEnumerator FadeText(float _targetAlpha)
    {
        if (messageText == null) yield break; // Safety check

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

    [Header("Save Notification")]
    [SerializeField] private TextMeshProUGUI saveNotificationText;
    [SerializeField] private float saveTextDuration = 2.0f;

    public void ShowSaveNotification()
    {
        if (saveNotificationText == null) return;
        
        StartCoroutine(SaveTextRoutine());
    }

    private IEnumerator SaveTextRoutine()
    {
        saveNotificationText.gameObject.SetActive(true);
        saveNotificationText.alpha = 1;
        yield return new WaitForSeconds(saveTextDuration);
        
        // Fade out
        float timer = 0;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            saveNotificationText.alpha = Mathf.Lerp(1, 0, timer);
            yield return null;
        }
        
        saveNotificationText.gameObject.SetActive(false);
    }
}
