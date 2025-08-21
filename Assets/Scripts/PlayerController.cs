// Importa bibliotecas necess�rias. NUnit.Framework � para testes, mas n�o � usado aqui.
// System.Collections.Generic � para usar Listas (List<T>).
// UnityEngine e UnityEngine.InputSystem s�o essenciais para a funcionalidade da Unity.
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // --- Padr�o Singleton ---
    // 'instance' � uma vari�vel est�tica que guarda a �nica refer�ncia do PlayerController no jogo.
    // Isso permite que qualquer outro script acesse o jogador facilmente (ex: Enemy.cs, GameManager.cs)
    // chamando PlayerController.instance.
    public static PlayerController instance;

    // --- Componentes ---
    // O [Header] organiza os campos no Inspector da Unity para facilitar a visualiza��o.
    [Header("Components")]
    [SerializeField] private Animator animator; // Refer�ncia ao componente Animator para controlar as anima��es.
    [SerializeField] private Rigidbody2D rb; // Refer�ncia ao Rigidbody2D para controlar a f�sica e o movimento.
    private UIController ui; // Refer�ncia ao script que controla a interface do usu�rio (vida, XP, etc).

    // --- Atributos do Jogador ---
    [Header("Stats")]
    [SerializeField] private int speed; // Velocidade de movimento do jogador.
    [SerializeField] public float maxHealth; // Vida m�xima do jogador.
    [SerializeField] private float immunityDuration; // Dura��o (em segundos) da invencibilidade ap�s tomar dano.
    private float immunityTimer; // Cron�metro para controlar o tempo restante de invencibilidade.

    public float currentHealth; // Vida atual do jogador.
    private bool isImune; // Flag (booleano) que indica se o jogador est� invenc�vel no momento.
    private Vector2 movement; // Vetor que armazena a dire��o do input de movimento (ex: (1, 0) para direita).

    // --- Sistema de XP ---
    [Header("XP system")]
    public int currentLevel; // N�vel atual do jogador.
    public int maxLevel; // N�vel m�ximo que o jogador pode atingir.
    public int currentExperience; // Quantidade de XP atual do jogador.
    // Lista que define quanto XP � necess�rio para passar para o pr�ximo n�vel.
    // Ex: playerLevels[0] = 100 XP para ir do n�vel 0 para o 1.
    public List<int> playerLevels;

    // Arma ativa do jogador.
    public Weapon activeWeapon;

    // --- M�todos de Ciclo de Vida do Unity ---

    // Awake � chamado antes de qualquer m�todo Start, assim que o objeto � criado.
    // Ideal para inicializar o Singleton.
    void Awake()
    {
        // Se n�o existe nenhuma inst�ncia do PlayerController ainda...
        if (instance == null)
        {
            // ...esta se torna a inst�ncia principal.
            instance = this;
        }
        else
        {
            // Se j� existe uma inst�ncia, esta c�pia � destru�da para garantir que haja apenas uma.
            Destroy(gameObject);
        }
    }

    // Start � chamado uma vez, ap�s o Awake, antes do primeiro frame do jogo.
    // Usado para configurar refer�ncias e estados iniciais.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pega a refer�ncia do componente Rigidbody2D anexado a este GameObject.
        ui = UIController.instance; // Pega a inst�ncia do controlador de UI.
        currentHealth = maxHealth; // Define a vida inicial como a vida m�xima.

        // Atualiza a barra de vida na UI no in�cio do jogo.
        // O '?' � um "null-conditional operator", que evita erros se 'ui' for nulo.
        ui?.UpdateHealthSlider();
    }

    // Update � chamado uma vez por frame. Ideal para l�gicas que n�o envolvem f�sica.
    private void Update()
    {
        // Controla o cron�metro de imunidade a cada frame.
        HandleImmunityDuration();
    }

    // FixedUpdate � chamado em intervalos de tempo fixos. � o local correto para manipular a f�sica.
    private void FixedUpdate()
    {
        // Aplica o movimento ao jogador.
        Move();
    }

    // --- L�gica de Imunidade ---

    // Controla a dura��o da imunidade ap�s receber dano.
    private void HandleImmunityDuration()
    {
        // Se o cron�metro estiver rodando...
        if (immunityTimer > 0)
        {
            // ...diminui o tempo usando Time.deltaTime (tempo decorrido desde o �ltimo frame).
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            // Se o tempo acabou, o jogador n�o est� mais imune.
            isImune = false;
        }
    }

    // --- L�gica de Movimento ---

    // Este m�todo � chamado pelo novo Input System da Unity sempre que uma a��o de movimento � detectada.
    public void OnMove(InputValue inputValue)
    {
        // Pega o valor do input (um Vector2 do teclado/controle) e o armazena.
        movement = inputValue.Get<Vector2>();
        // Atualiza o Animator com base na dire��o do movimento.
        UpdateAnimator(movement);
    }

    // Aplica a for�a de movimento ao Rigidbody2D.
    private void Move()
    {
        // Define a velocidade linear do corpo r�gido.
        // .normalized garante que o vetor de movimento tenha sempre comprimento 1,
        // evitando que o jogador se mova mais r�pido na diagonal.
        rb.linearVelocity = movement.normalized * speed;
    }

    // Atualiza os par�metros do Animator para tocar a anima��o correta.
    private void UpdateAnimator(Vector2 movement)
    {
        // Envia os valores X e Y do movimento para o Animator.
        // O Animator usar� esses valores para decidir entre anima��es de andar para cima, baixo, esquerda, direita.
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        // Define um booleano no Animator. Se o vetor de movimento for diferente de zero, "isMoving" � verdadeiro.
        // Isso � usado para alternar entre a anima��o de "parado" (idle) e "andando".
        animator.SetBool("isMoving", movement != Vector2.zero);
    }

    // --- L�gica de Combate e Dano ---

    // M�todo p�blico para que outros objetos (inimigos, proj�teis) possam causar dano ao jogador.
    public void TakeDamage(float damage)
    {
        // Se o jogador j� est� imune ou o dano � zero ou negativo, o m�todo n�o faz nada.
        if (isImune || damage <= 0) return;

        // Ativa a imunidade e define o cron�metro.
        isImune = true;
        immunityTimer = immunityDuration;

        // Reduz a vida atual e atualiza a UI.
        currentHealth -= damage;
        ui?.UpdateHealthSlider();

        // Verifica se a vida chegou a zero.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Gerencia a morte do jogador.
    private void Die()
    {
        // Desativa o GameObject do jogador, fazendo-o desaparecer da cena.
        gameObject.SetActive(false);
        // Chama o m�todo GameOver no GameManager para encerrar o jogo.
        GameManager.instance.GameOver();
    }

    // --- L�gica de Progress�o ---

    // M�todo para adicionar experi�ncia ao jogador.
    public void GetExperience(int experienceToGet)
    {
        currentExperience += experienceToGet;
        ui.UpdateExperienceSlider(); // Atualiza a barra de XP na UI.

        // Verifica se a experi�ncia atual atingiu o necess�rio para o pr�ximo n�vel.
        if (currentExperience >= playerLevels[currentLevel])
        {
            LevelUp();
        }
    }

    // Gerencia a subida de n�vel do jogador.
    public void LevelUp()
    {
        // Subtrai o XP necess�rio para o n�vel atual, mantendo o excedente para o pr�ximo.
        currentExperience -= playerLevels[currentLevel];
        currentLevel++; // Incrementa o n�vel.
        ui.UpdateExperienceSlider(); // Atualiza a UI com o novo n�vel e XP.

        // Ativa o primeiro bot�o de upgrade na UI, passando a arma atual como op��o.
        ui.levelUpButtons[0].ActivateButton(activeWeapon);

        // Abre o painel de level up para o jogador escolher uma melhoria.
        ui.LevelUpPanelOpen();
    }
}
