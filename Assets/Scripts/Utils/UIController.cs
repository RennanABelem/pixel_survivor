// Importa as bibliotecas necess�rias.
// TMPro � para usar os componentes de texto avan�ados (TextMeshPro).
// UnityEngine � a biblioteca principal.
// UnityEngine.UI cont�m componentes de UI como Slider e Button.
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // --- Padr�o Singleton ---
    // 'instance' garante uma �nica refer�ncia global para o UIController,
    // permitindo que outros scripts (PlayerController, GameManager) o acessem facilmente.
    public static UIController instance;

    // --- Elementos da UI de Vida ---
    [Header("Health UI")]
    [SerializeField] private Slider playerHealthSlider; // A barra de vida visual.
    [SerializeField] private TMP_Text healthText, timerText; // Textos para exibir vida (ex: "100/100") e o cron�metro.

    // --- Elementos da UI de Experi�ncia ---
    [Header("Experience UI")]
    [SerializeField] private Slider playerExperienceSlider; // A barra de XP visual.
    [SerializeField] private TMP_Text experienceText; // Texto para exibir o XP (ex: "50/200").

    // --- Pain�is da UI ---
    [Header("Panels")]
    [SerializeField] public GameObject gameOverPanel; // Painel exibido quando o jogador morre.
    [SerializeField] public GameObject pausePanel; // Painel exibido quando o jogo � pausado.
    [SerializeField] public GameObject levelUpPanel; // Painel para o jogador escolher upgrades ao subir de n�vel.

    // Um array (lista) de bot�es que aparecem na tela de level up.
    public LevelUpButton[] levelUpButtons;

    // Refer�ncia privada ao controlador do jogador para buscar dados como vida e XP.
    private PlayerController player;

    // Awake � chamado antes de qualquer m�todo Start.
    void Awake()
    {
        // Configura��o do Singleton.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Pega a inst�ncia do jogador. Se n�o encontrar, avisa no console.
        player = PlayerController.instance;
        if (player == null)
        {
            Debug.LogWarning("PlayerController nao encontrado.");
        }

        // Chama os m�todos de atualiza��o no in�cio para garantir que a UI
        // comece com os valores corretos de vida e XP do jogador.
        UpdateExperienceSlider();
        UpdateHealthSlider();
    }

    // Atualiza a barra de vida e o texto correspondente.
    // � chamado no in�cio e sempre que o jogador toma dano.
    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = player.maxHealth; // O valor m�ximo da barra � a vida m�xima do jogador.
        playerHealthSlider.value = player.currentHealth; // O preenchimento atual da barra � a vida atual.

        // Usa uma string interpolada ($"...") para formatar o texto de forma limpa e leg�vel.
        healthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }

    // Atualiza a barra de experi�ncia e o texto correspondente.
    // � chamado no in�cio e sempre que o jogador ganha XP.
    public void UpdateExperienceSlider()
    {
        // O valor m�ximo da barra � o XP necess�rio para o n�vel atual do jogador.
        playerExperienceSlider.maxValue = player.playerLevels[player.currentLevel];
        playerExperienceSlider.value = player.currentExperience; // O preenchimento � o XP atual.

        experienceText.text = $"{player.currentExperience} / {player.playerLevels[player.currentLevel]}";
    }

    // Atualiza o texto do cron�metro. � chamado a cada frame pelo GameManager.
    public void UpdateTimerText(float timer)
    {
        // Calcula os minutos arredondando o total de segundos dividido por 60 para baixo.
        int min = Mathf.FloorToInt(timer / 60f);
        // Calcula os segundos usando o operador de m�dulo (%), que retorna o resto da divis�o.
        int sec = Mathf.FloorToInt(timer % 60f);

        // Formata o texto para o formato "MM:SS".
        // O "00" garante que os segundos sempre tenham dois d�gitos (ex: "07" em vez de "7").
        timerText.text = min.ToString() + ":" + sec.ToString("00");
    }

    // Abre o painel de Level Up e pausa o jogo.
    public void LevelUpPanelOpen()
    {
        levelUpPanel.SetActive(true); // Torna o painel vis�vel.
        Time.timeScale = 0; // Pausa o jogo (f�sica, anima��es, etc.).
    }

    // Fecha o painel de Level Up e despausa o jogo.
    public void LevelUpPanelClose()
    {
        levelUpPanel.SetActive(false); // Esconde o painel.
        Time.timeScale = 1f; // Retoma o tempo ao normal.
    }
}