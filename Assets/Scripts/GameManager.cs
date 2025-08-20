using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private GameControl inputActions;
    private UIController ui;
    public float gameTimer;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            ui = UIController.instance;
            if (ui == null)
            {
                Debug.LogError("UIController.instance não encontrado.");
            }

            InitializeInput();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!ui.gameOverPanel.activeSelf && !ui.pausePanel.activeSelf)
        {
            gameTimer += Time.deltaTime;
            ui.UpdateTimerText(gameTimer);
        }
    }

    private void InitializeInput()
    {
        inputActions = new GameControl();
        inputActions.Enable();

        inputActions.Game.Pause.performed += ctx => Pause();
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());

    }

    private IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1.5f);

        if (ui != null && ui.gameOverPanel != null)
        {
            ui.gameOverPanel.SetActive(true);
        }
    }

    public void Pause()
    {
        if (ui == null || ui.gameOverPanel == null || ui.pausePanel == null)
            return;

        if (ui.gameOverPanel.activeSelf)
            return;

        bool isPaused = ui.pausePanel.activeSelf;
        ui.pausePanel.SetActive(!isPaused);

        Time.timeScale = isPaused ? 1f : 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Game.Pause.performed -= ctx => Pause();
            inputActions.Dispose();
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
