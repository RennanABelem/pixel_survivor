using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Health UI")]
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TMP_Text healthText, timerText;

    [Header("Experience UI")]
    [SerializeField] private Slider playerExperienceSlider;
    [SerializeField] private TMP_Text experienceText;

    [Header("Panels")]
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject levelUpPanel;

    public LevelUpButton[] levelUpButtons;

    private PlayerController player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        player = PlayerController.instance;
        if (player == null)
        {
            Debug.LogWarning("PlayerController nao encontrado.");
        }

        UpdateExperienceSlider();
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = player.maxHealth;
        playerHealthSlider.value = player.currentHealth;

        healthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }

    public void UpdateExperienceSlider()
    {
        playerExperienceSlider.maxValue = player.playerLevels[player.currentLevel];
        playerExperienceSlider.value = player.currentExperience;

        experienceText.text = $"{player.currentExperience} / {player.playerLevels[player.currentLevel]}";
    }

    public void UpdateTimerText(float timer)
    {
        int min = Mathf.FloorToInt(timer / 60f);
        int sec = Mathf.FloorToInt(timer % 60f);

        timerText.text = min.ToString() + ":" + sec.ToString("00");
    }

    public void LevelUpPanelOpen()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void LevelUpPanelClose()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
