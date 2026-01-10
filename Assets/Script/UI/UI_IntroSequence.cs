using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_IntroSequence : MonoBehaviour, ISaveManager
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI skipHintText; // 新增：提示文本
    [SerializeField] private AudioClip introAudio; 

    [Header("Story Settings")]
    [TextArea(3, 10)]
    [SerializeField] private string fullStoryText = "很久很久以前...\n世界陷入了黑暗...";
    [SerializeField] private float typingSpeed = 0.05f;    
    [SerializeField] private float waitAfterTyping = 2.0f; 
    [SerializeField] private float fadeDuration = 1.5f;   
    [SerializeField] [Range(0f, 1f)] private float soundVolume = 1f;

    [Header("Hint Settings")]
    [SerializeField] private float blinkSpeed = 2.0f; // 闪烁速度

    private AudioSource audioSource;
    private bool isIntroOver = false;

    private void Start()
    {
        // 如果在 LoadData 中已经被禁用，则直接返回
        if (!this.gameObject.activeSelf) return;

        audioSource = GetComponent<AudioSource>();

        if (blackScreenCanvasGroup != null)
        {
            blackScreenCanvasGroup.alpha = 1;
            blackScreenCanvasGroup.blocksRaycasts = true; 
        }

        if (storyText != null) storyText.text = "";
        if (skipHintText != null) skipHintText.gameObject.SetActive(true);

        if (audioSource != null && introAudio != null)
        {
            audioSource.clip = introAudio;
            audioSource.volume = soundVolume;
            audioSource.loop = true; 
            audioSource.Play();
        }

        StartCoroutine(PlayIntroSequence());
        StartCoroutine(BlinkSkipHint());
    }

    // ISaveManager Implementation
    public void LoadData(GameData _data)
    {
        // If it's NOT the first time (e.g. Continue Game), skip the intro entirely
        if (!_data.isFirstLoad)
        {
            if (blackScreenCanvasGroup != null)
            {
                blackScreenCanvasGroup.alpha = 0;
                blackScreenCanvasGroup.blocksRaycasts = false;
            }
            // Disable this object effectively disabling the intro
            this.gameObject.SetActive(false); 
        }
    }

    public void SaveData(ref GameData _data)
    {
        // No data to save
    }

    // ... Rest of the class ...
    private IEnumerator BlinkSkipHint()
    {
        while (!isIntroOver)
        {
            if (skipHintText != null)
            {
                // 使用 PingPong 实现呼吸/闪烁效果 (Alpha 0 ~ 1)
                skipHintText.alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            }
            yield return null;
        }
        
        // 结束后隐藏
        if (skipHintText != null)
            skipHintText.alpha = 0;
    }

    private IEnumerator PlayIntroSequence()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < fullStoryText.Length; i++)
        {
            if (Input.anyKey)
            {
                if (storyText != null) storyText.text = fullStoryText;
                break;
            }

            if (storyText != null)
                storyText.text += fullStoryText[i];
            
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(waitAfterTyping);

        // 标记结束，停止闪烁
        isIntroOver = true;

        float timer = 0;
        float startVolume = audioSource != null ? audioSource.volume : 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            if (blackScreenCanvasGroup != null)
                blackScreenCanvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            
            if (audioSource != null)
                audioSource.volume = Mathf.Lerp(startVolume, 0, progress);

            yield return null;
        }

        if (blackScreenCanvasGroup != null)
        {
            blackScreenCanvasGroup.alpha = 0;
            blackScreenCanvasGroup.blocksRaycasts = false; 
            blackScreenCanvasGroup.gameObject.SetActive(false); 
        }
    }
}
