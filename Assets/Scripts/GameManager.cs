// Importa bibliotecas necess�rias da Unity.
// System.Collections � para usar Coroutines (IEnumerator).
// UnityEngine � a biblioteca principal.
// UnityEngine.SceneManagement � essencial para carregar e gerenciar cenas (fases, menus).
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Padr�o Singleton ---
    // Assim como no PlayerController, 'instance' garante que exista apenas um GameManager no jogo,
    // permitindo acesso global f�cil atrav�s de GameManager.instance.
    public static GameManager instance;

    // --- Refer�ncias e Vari�veis ---
    private GameControl inputActions; // Objeto que representa o asset de controles (novo Input System).
    private UIController ui; // Refer�ncia ao script que controla a UI.
    public float gameTimer; // Cron�metro para a partida atual.

    // --- M�todos de Ciclo de Vida do Unity ---

    // Awake � chamado quando o script � carregado, antes do Start.
    void Awake()
    {
        // Implementa��o do Singleton.
        if (instance == null)
        {
            instance = this;

            // Pega a inst�ncia do UIController. � uma boa pr�tica verificar se a refer�ncia foi encontrada.
            ui = UIController.instance;
            if (ui == null)
            {
                // Se n�o encontrar, exibe um erro no console da Unity para facilitar a depura��o.
                Debug.LogError("UIController.instance n�o encontrado.");
            }

            // Inicializa o sistema de input para o GameManager.
            InitializeInput();
        }
        else
        {
            // Se j� existe uma inst�ncia, destr�i este objeto para evitar duplicatas.
            Destroy(gameObject);
        }
    }

    // Update � chamado a cada frame.
    private void Update()
    {
        // O cron�metro s� avan�a se o jogo n�o estiver na tela de Game Over e nem pausado.
        if (!ui.gameOverPanel.activeSelf && !ui.pausePanel.activeSelf)
        {
            // Adiciona o tempo decorrido desde o �ltimo frame ao cron�metro.
            gameTimer += Time.deltaTime;
            // Pede para a UI atualizar o texto do cron�metro na tela.
            ui.UpdateTimerText(gameTimer);
        }
    }

    // --- L�gica de Inicializa��o ---

    private void InitializeInput()
    {
        // Cria uma nova inst�ncia do nosso mapa de controles.
        inputActions = new GameControl();
        // Ativa o mapa de controles para que ele possa detectar inputs.
        inputActions.Enable();

        // "Inscreve" o m�todo Pause() para ser chamado sempre que a a��o "Pause" for executada (performed).
        // Isso cria um v�nculo direto entre o bot�o de pausa e a fun��o que pausa o jogo.
        inputActions.Game.Pause.performed += ctx => Pause();
    }

    // --- Gerenciamento de Estado do Jogo ---

    // M�todo p�blico chamado por outro script (provavelmente PlayerController) quando o jogador morre.
    public void GameOver()
    {
        // Inicia uma Coroutine para mostrar a tela de Game Over com um pequeno atraso.
        StartCoroutine(ShowGameOverScreen());
    }

    // Uma Coroutine permite executar l�gicas com pausas (esperas).
    private IEnumerator ShowGameOverScreen()
    {
        // Espera por 1.5 segundos antes de continuar a execu��o. Isso evita um "Game Over" abrupto.
        yield return new WaitForSeconds(1.5f);

        // Ap�s a espera, ativa o painel de Game Over na UI.
        if (ui != null && ui.gameOverPanel != null)
        {
            ui.gameOverPanel.SetActive(true);
        }
    }

    // Controla a pausa do jogo.
    public void Pause()
    {
        // Verifica��es de seguran�a para evitar erros se algum componente da UI n�o estiver atribu�do.
        if (ui == null || ui.gameOverPanel == null || ui.pausePanel == null)
            return;

        // N�o permite pausar se a tela de Game Over j� estiver ativa.
        if (ui.gameOverPanel.activeSelf)
            return;

        // Verifica se o jogo j� est� pausado (se o painel de pausa est� ativo).
        bool isPaused = ui.pausePanel.activeSelf;
        // Inverte o estado: se estava ativo, desativa; se estava inativo, ativa.
        ui.pausePanel.SetActive(!isPaused);

        // A "m�gica" da pausa: Time.timeScale controla a velocidade com que o tempo passa no jogo.
        // Se o jogo estava pausado (isPaused = true), volta o tempo ao normal (1f).
        // Se n�o estava pausado, congela o tempo (0f), pausando f�sica, anima��es, etc.
        Time.timeScale = isPaused ? 1f : 0f;
    }

    // --- Navega��o e Cenas ---

    // Reinicia a cena atual.
    public void Restart()
    {
        // Garante que o tempo volte ao normal antes de carregar a nova cena.
        Time.timeScale = 1f;
        // Carrega a cena chamada "Game".
        SceneManager.LoadScene("Game");
    }

    // Volta para o menu principal.
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Fecha o jogo.
    public void QuitGame()
    {
        // Esta fun��o s� funciona quando o jogo est� compilado (build), n�o funciona no Editor da Unity.
        Application.Quit();
    }

    // --- Limpeza ---

    // OnDestroy � chamado quando o objeto � destru�do (ex: ao trocar de cena).
    // � crucial para limpar "inscri��es" de eventos e evitar vazamentos de mem�ria.
    private void OnDestroy()
    {
        if (inputActions != null)
        {
            // "Desinscreve" o m�todo Pause do evento. Se isso n�o for feito, podem ocorrer erros.
            inputActions.Game.Pause.performed -= ctx => Pause();
            // Libera os recursos usados pelo objeto de input.
            inputActions.Dispose();
        }
    }
}
