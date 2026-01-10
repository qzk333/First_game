using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private const string VolumePrefKey = "MasterVol_Linear";
    private const float DefaultVolume = 0.5f;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Settings Components")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    private void Awake()
    {
        // Fallback auto-assign if the slider wasn't wired in the inspector
        if (volumeSlider == null && settingsPanel != null)
            volumeSlider = settingsPanel.GetComponentInChildren<Slider>(true);
    }

    private void Start()
    {
        // Ensure main menu is visible on load
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // Align slider with current volume on open
        SyncVolumeFromStorage();
    }

    public void OnStartGame()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.NewGame();
        }

        SceneManager.LoadScene(1);
    }

    public void OnContinueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        // Refresh slider to reflect current volume when settings opens
        SyncVolumeSlider();
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnQuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

    public void SetVolume(float volume)
    {
        ApplyVolume(volume);

        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    private void SyncVolumeFromStorage()
    {
        float currentVolume = GetSavedVolume();

        ApplyVolume(currentVolume);

        if (volumeSlider != null)
            volumeSlider.SetValueWithoutNotify(currentVolume);
    }

    private void SyncVolumeSlider()
    {
        if (volumeSlider == null)
            return;

        float currentVolume = GetSavedVolume();
        volumeSlider.SetValueWithoutNotify(currentVolume);
    }

    private float GetSavedVolume()
    {
        if (PlayerPrefs.HasKey(VolumePrefKey))
            return PlayerPrefs.GetFloat(VolumePrefKey);

        return DefaultVolume;
    }

    private float GetRuntimeVolume()
    {
        if (audioMixer != null && audioMixer.GetFloat("MasterVol", out float dbVal))
            return Mathf.Pow(10, dbVal / 20f);

        return AudioListener.volume;
    }

    private void ApplyVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVol", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        }
        else
        {
            AudioListener.volume = volume;
        }
    }
}
