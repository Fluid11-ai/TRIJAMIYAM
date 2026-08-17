using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool levelEnded = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        HideAllPanels();
    }

    // =========================
    // GAME FLOW
    // =========================

    public void LevelComplete()
    {
        if (levelEnded) return;
        levelEnded = true;

        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void LevelFailed()
    {
        if (levelEnded) return;
        levelEnded = true;

        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    // =========================
    // BUTTON CALLBACKS
    // =========================

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        levelEnded = false;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        levelEnded = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // =========================
    // UTILITY
    // =========================

    void HideAllPanels()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
}
