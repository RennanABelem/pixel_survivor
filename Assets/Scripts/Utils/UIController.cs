// Importa as bibliotecas necessárias.
// TMPro é para usar os componentes de texto avançados (TextMeshPro).
// UnityEngine é a biblioteca principal.
// UnityEngine.UI contém componentes de UI como Slider e Button.
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // --- Padrão Singleton ---
    // 'instance' garante uma única referência global para o UIController,
    // permitindo que outros scripts (PlayerController, GameManager) o acessem facilmente.
    public static UIController instance;

    // --- Elementos da UI de Vida ---
    [Header("Health UI")]
    [SerializeField] private Slider playerHealthSlider; // A barra de vida visual.
    [SerializeField] private TMP_Text healthText, timerText; // Textos para exibir vida (ex: "100/100") e o cronômetro.

    // --- Elementos da UI de Experiência ---
    [Header("Experience UI")]
    [SerializeField] private Slider playerExperienceSlider; // A barra de XP visual.
    [SerializeField] private TMP_Text experienceText; // Texto para exibir o XP (ex: "50/200").

    // --- Painéis da UI ---
    [Header("Panels")]
    [SerializeField] public GameObject gameOverPanel; // Painel exibido quando o jogador morre.
    [SerializeField] public GameObject pausePanel; // Painel exibido quando o jogo é pausado.
    [SerializeField] public GameObject levelUpPanel; // Painel para o jogador escolher upgrades ao subir de nível.

    // Um array (lista) de botões que aparecem na tela de level up.
    public LevelUpButton[] levelUpButtons;

    // Referência privada ao controlador do jogador para buscar dados como vida e XP.
    private PlayerController player;

    // Awake é chamado antes de qualquer método Start.
    void Awake()
    {
        // Configuração do Singleton.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Pega a instância do jogador. Se não encontrar, avisa no console.
        player = PlayerController.instance;
        if (player == null)
        {
            Debug.LogWarning("PlayerController nao encontrado.");
        }

        // Chama os métodos de atualização no início para garantir que a UI
        // comece com os valores corretos de vida e XP do jogador.
        UpdateExperienceSlider();
        UpdateHealthSlider();
    }

    // Atualiza a barra de vida e o texto correspondente.
    // É chamado no início e sempre que o jogador toma dano.
    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = player.maxHealth; // O valor máximo da barra é a vida máxima do jogador.
        playerHealthSlider.value = player.currentHealth; // O preenchimento atual da barra é a vida atual.

        // Usa uma string interpolada ($"...") para formatar o texto de forma limpa e legível.
        healthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }

    // Atualiza a barra de experiência e o texto correspondente.
    // É chamado no início e sempre que o jogador ganha XP.
    public void UpdateExperienceSlider()
    {
        // O valor máximo da barra é o XP necessário para o nível atual do jogador.
        playerExperienceSlider.maxValue = player.playerLevels[player.currentLevel];
        playerExperienceSlider.value = player.currentExperience; // O preenchimento é o XP atual.

        experienceText.text = $"{player.currentExperience} / {player.playerLevels[player.currentLevel]}";
    }

    // Atualiza o texto do cronômetro. É chamado a cada frame pelo GameManager.
    public void UpdateTimerText(float timer)
    {
        // Calcula os minutos arredondando o total de segundos dividido por 60 para baixo.
        int min = Mathf.FloorToInt(timer / 60f);
        // Calcula os segundos usando o operador de módulo (%), que retorna o resto da divisão.
        int sec = Mathf.FloorToInt(timer % 60f);

        // Formata o texto para o formato "MM:SS".
        // O "00" garante que os segundos sempre tenham dois dígitos (ex: "07" em vez de "7").
        timerText.text = min.ToString() + ":" + sec.ToString("00");
    }

    // Abre o painel de Level Up e pausa o jogo.
    public void LevelUpPanelOpen()
    {
        levelUpPanel.SetActive(true); // Torna o painel visível.
        Time.timeScale = 0; // Pausa o jogo (física, animações, etc.).
    }

    // Fecha o painel de Level Up e despausa o jogo.
    public void LevelUpPanelClose()
    {
        levelUpPanel.SetActive(false); // Esconde o painel.
        Time.timeScale = 1f; // Retoma o tempo ao normal.
    }
}