// Importa bibliotecas necessárias da Unity.
// System.Collections é para usar Coroutines (IEnumerator).
// UnityEngine é a biblioteca principal.
// UnityEngine.SceneManagement é essencial para carregar e gerenciar cenas (fases, menus).
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Padrão Singleton ---
    // Assim como no PlayerController, 'instance' garante que exista apenas um GameManager no jogo,
    // permitindo acesso global fácil através de GameManager.instance.
    public static GameManager instance;

    // --- Referências e Variáveis ---
    private GameControl inputActions; // Objeto que representa o asset de controles (novo Input System).
    private UIController ui; // Referência ao script que controla a UI.
    public float gameTimer; // Cronômetro para a partida atual.

    // --- Métodos de Ciclo de Vida do Unity ---

    // Awake é chamado quando o script é carregado, antes do Start.
    void Awake()
    {
        // Implementação do Singleton.
        if (instance == null)
        {
            instance = this;

            // Pega a instância do UIController. É uma boa prática verificar se a referência foi encontrada.
            ui = UIController.instance;
            if (ui == null)
            {
                // Se não encontrar, exibe um erro no console da Unity para facilitar a depuração.
                Debug.LogError("UIController.instance não encontrado.");
            }

            // Inicializa o sistema de input para o GameManager.
            InitializeInput();
        }
        else
        {
            // Se já existe uma instância, destrói este objeto para evitar duplicatas.
            Destroy(gameObject);
        }
    }

    // Update é chamado a cada frame.
    private void Update()
    {
        // O cronômetro só avança se o jogo não estiver na tela de Game Over e nem pausado.
        if (!ui.gameOverPanel.activeSelf && !ui.pausePanel.activeSelf)
        {
            // Adiciona o tempo decorrido desde o último frame ao cronômetro.
            gameTimer += Time.deltaTime;
            // Pede para a UI atualizar o texto do cronômetro na tela.
            ui.UpdateTimerText(gameTimer);
        }
    }

    // --- Lógica de Inicialização ---

    private void InitializeInput()
    {
        // Cria uma nova instância do nosso mapa de controles.
        inputActions = new GameControl();
        // Ativa o mapa de controles para que ele possa detectar inputs.
        inputActions.Enable();

        // "Inscreve" o método Pause() para ser chamado sempre que a ação "Pause" for executada (performed).
        // Isso cria um vínculo direto entre o botão de pausa e a função que pausa o jogo.
        inputActions.Game.Pause.performed += ctx => Pause();
    }

    // --- Gerenciamento de Estado do Jogo ---

    // Método público chamado por outro script (provavelmente PlayerController) quando o jogador morre.
    public void GameOver()
    {
        // Inicia uma Coroutine para mostrar a tela de Game Over com um pequeno atraso.
        StartCoroutine(ShowGameOverScreen());
    }

    // Uma Coroutine permite executar lógicas com pausas (esperas).
    private IEnumerator ShowGameOverScreen()
    {
        // Espera por 1.5 segundos antes de continuar a execução. Isso evita um "Game Over" abrupto.
        yield return new WaitForSeconds(1.5f);

        // Após a espera, ativa o painel de Game Over na UI.
        if (ui != null && ui.gameOverPanel != null)
        {
            ui.gameOverPanel.SetActive(true);
        }
    }

    // Controla a pausa do jogo.
    public void Pause()
    {
        // Verificações de segurança para evitar erros se algum componente da UI não estiver atribuído.
        if (ui == null || ui.gameOverPanel == null || ui.pausePanel == null)
            return;

        // Não permite pausar se a tela de Game Over já estiver ativa.
        if (ui.gameOverPanel.activeSelf)
            return;

        // Verifica se o jogo já está pausado (se o painel de pausa está ativo).
        bool isPaused = ui.pausePanel.activeSelf;
        // Inverte o estado: se estava ativo, desativa; se estava inativo, ativa.
        ui.pausePanel.SetActive(!isPaused);

        // A "mágica" da pausa: Time.timeScale controla a velocidade com que o tempo passa no jogo.
        // Se o jogo estava pausado (isPaused = true), volta o tempo ao normal (1f).
        // Se não estava pausado, congela o tempo (0f), pausando física, animações, etc.
        Time.timeScale = isPaused ? 1f : 0f;
    }

    // --- Navegação e Cenas ---

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
        // Esta função só funciona quando o jogo está compilado (build), não funciona no Editor da Unity.
        Application.Quit();
    }

    // --- Limpeza ---

    // OnDestroy é chamado quando o objeto é destruído (ex: ao trocar de cena).
    // É crucial para limpar "inscrições" de eventos e evitar vazamentos de memória.
    private void OnDestroy()
    {
        if (inputActions != null)
        {
            // "Desinscreve" o método Pause do evento. Se isso não for feito, podem ocorrer erros.
            inputActions.Game.Pause.performed -= ctx => Pause();
            // Libera os recursos usados pelo objeto de input.
            inputActions.Dispose();
        }
    }
}
