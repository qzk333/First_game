using UnityEngine;

public class GameOverPanelController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Player.OnPlayerDied += ShowGameOver;
    }

    private void OnDisable()
    {
        Player.OnPlayerDied -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
