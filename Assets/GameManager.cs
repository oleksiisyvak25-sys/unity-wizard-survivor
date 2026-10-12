using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject inGameUIPanel;

    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Score = 0;
        UpdateScoreDisplay();

        if (startMenuPanel != null)
        {
            Time.timeScale = 0f;
            startMenuPanel.SetActive(true);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (inGameUIPanel != null) inGameUIPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            if (inGameUIPanel != null) inGameUIPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (inGameUIPanel != null) inGameUIPanel.SetActive(true);
    }

    public void AddScore(int amount = 1)
    {
        Score += amount;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (inGameScoreText != null)
        {
            inGameScoreText.text = $"Kills: {Score}";
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        if (inGameUIPanel != null) inGameUIPanel.SetActive(false);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Defeated: {Score}";
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}