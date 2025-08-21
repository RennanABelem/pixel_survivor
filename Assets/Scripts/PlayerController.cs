// Importa bibliotecas necessárias. NUnit.Framework é para testes, mas não é usado aqui.
// System.Collections.Generic é para usar Listas (List<T>).
// UnityEngine e UnityEngine.InputSystem são essenciais para a funcionalidade da Unity.
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // --- Padrão Singleton ---
    // 'instance' é uma variável estática que guarda a única referência do PlayerController no jogo.
    // Isso permite que qualquer outro script acesse o jogador facilmente (ex: Enemy.cs, GameManager.cs)
    // chamando PlayerController.instance.
    public static PlayerController instance;

    // --- Componentes ---
    // O [Header] organiza os campos no Inspector da Unity para facilitar a visualização.
    [Header("Components")]
    [SerializeField] private Animator animator; // Referência ao componente Animator para controlar as animações.
    [SerializeField] private Rigidbody2D rb; // Referência ao Rigidbody2D para controlar a física e o movimento.
    private UIController ui; // Referência ao script que controla a interface do usuário (vida, XP, etc).

    // --- Atributos do Jogador ---
    [Header("Stats")]
    [SerializeField] private int speed; // Velocidade de movimento do jogador.
    [SerializeField] public float maxHealth; // Vida máxima do jogador.
    [SerializeField] private float immunityDuration; // Duração (em segundos) da invencibilidade após tomar dano.
    private float immunityTimer; // Cronômetro para controlar o tempo restante de invencibilidade.

    public float currentHealth; // Vida atual do jogador.
    private bool isImune; // Flag (booleano) que indica se o jogador está invencível no momento.
    private Vector2 movement; // Vetor que armazena a direção do input de movimento (ex: (1, 0) para direita).

    // --- Sistema de XP ---
    [Header("XP system")]
    public int currentLevel; // Nível atual do jogador.
    public int maxLevel; // Nível máximo que o jogador pode atingir.
    public int currentExperience; // Quantidade de XP atual do jogador.
    // Lista que define quanto XP é necessário para passar para o próximo nível.
    // Ex: playerLevels[0] = 100 XP para ir do nível 0 para o 1.
    public List<int> playerLevels;

    // Arma ativa do jogador.
    public Weapon activeWeapon;

    // --- Métodos de Ciclo de Vida do Unity ---

    // Awake é chamado antes de qualquer método Start, assim que o objeto é criado.
    // Ideal para inicializar o Singleton.
    void Awake()
    {
        // Se não existe nenhuma instância do PlayerController ainda...
        if (instance == null)
        {
            // ...esta se torna a instância principal.
            instance = this;
        }
        else
        {
            // Se já existe uma instância, esta cópia é destruída para garantir que haja apenas uma.
            Destroy(gameObject);
        }
    }

    // Start é chamado uma vez, após o Awake, antes do primeiro frame do jogo.
    // Usado para configurar referências e estados iniciais.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pega a referência do componente Rigidbody2D anexado a este GameObject.
        ui = UIController.instance; // Pega a instância do controlador de UI.
        currentHealth = maxHealth; // Define a vida inicial como a vida máxima.

        // Atualiza a barra de vida na UI no início do jogo.
        // O '?' é um "null-conditional operator", que evita erros se 'ui' for nulo.
        ui?.UpdateHealthSlider();
    }

    // Update é chamado uma vez por frame. Ideal para lógicas que não envolvem física.
    private void Update()
    {
        // Controla o cronômetro de imunidade a cada frame.
        HandleImmunityDuration();
    }

    // FixedUpdate é chamado em intervalos de tempo fixos. É o local correto para manipular a física.
    private void FixedUpdate()
    {
        // Aplica o movimento ao jogador.
        Move();
    }

    // --- Lógica de Imunidade ---

    // Controla a duração da imunidade após receber dano.
    private void HandleImmunityDuration()
    {
        // Se o cronômetro estiver rodando...
        if (immunityTimer > 0)
        {
            // ...diminui o tempo usando Time.deltaTime (tempo decorrido desde o último frame).
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            // Se o tempo acabou, o jogador não está mais imune.
            isImune = false;
        }
    }

    // --- Lógica de Movimento ---

    // Este método é chamado pelo novo Input System da Unity sempre que uma ação de movimento é detectada.
    public void OnMove(InputValue inputValue)
    {
        // Pega o valor do input (um Vector2 do teclado/controle) e o armazena.
        movement = inputValue.Get<Vector2>();
        // Atualiza o Animator com base na direção do movimento.
        UpdateAnimator(movement);
    }

    // Aplica a força de movimento ao Rigidbody2D.
    private void Move()
    {
        // Define a velocidade linear do corpo rígido.
        // .normalized garante que o vetor de movimento tenha sempre comprimento 1,
        // evitando que o jogador se mova mais rápido na diagonal.
        rb.linearVelocity = movement.normalized * speed;
    }

    // Atualiza os parâmetros do Animator para tocar a animação correta.
    private void UpdateAnimator(Vector2 movement)
    {
        // Envia os valores X e Y do movimento para o Animator.
        // O Animator usará esses valores para decidir entre animações de andar para cima, baixo, esquerda, direita.
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        // Define um booleano no Animator. Se o vetor de movimento for diferente de zero, "isMoving" é verdadeiro.
        // Isso é usado para alternar entre a animação de "parado" (idle) e "andando".
        animator.SetBool("isMoving", movement != Vector2.zero);
    }

    // --- Lógica de Combate e Dano ---

    // Método público para que outros objetos (inimigos, projéteis) possam causar dano ao jogador.
    public void TakeDamage(float damage)
    {
        // Se o jogador já está imune ou o dano é zero ou negativo, o método não faz nada.
        if (isImune || damage <= 0) return;

        // Ativa a imunidade e define o cronômetro.
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
        // Chama o método GameOver no GameManager para encerrar o jogo.
        GameManager.instance.GameOver();
    }

    // --- Lógica de Progressão ---

    // Método para adicionar experiência ao jogador.
    public void GetExperience(int experienceToGet)
    {
        currentExperience += experienceToGet;
        ui.UpdateExperienceSlider(); // Atualiza a barra de XP na UI.

        // Verifica se a experiência atual atingiu o necessário para o próximo nível.
        if (currentExperience >= playerLevels[currentLevel])
        {
            LevelUp();
        }
    }

    // Gerencia a subida de nível do jogador.
    public void LevelUp()
    {
        // Subtrai o XP necessário para o nível atual, mantendo o excedente para o próximo.
        currentExperience -= playerLevels[currentLevel];
        currentLevel++; // Incrementa o nível.
        ui.UpdateExperienceSlider(); // Atualiza a UI com o novo nível e XP.

        // Ativa o primeiro botão de upgrade na UI, passando a arma atual como opção.
        ui.levelUpButtons[0].ActivateButton(activeWeapon);

        // Abre o painel de level up para o jogador escolher uma melhoria.
        ui.LevelUpPanelOpen();
    }
}
