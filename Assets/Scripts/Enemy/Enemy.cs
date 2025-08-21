using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Referência ao componente que renderiza o sprite, usado para virar o inimigo.
    [SerializeField] private Rigidbody2D rb;               // Referência ao corpo rígido para controlar a física e o movimento.

    [Header("Stats")]
    [SerializeField] private float speed;               // Velocidade de movimento do inimigo.
    [SerializeField] private int damage;                // Dano que o inimigo causa ao jogador por contato.
    [SerializeField] private float health;              // Pontos de vida do inimigo.
    [SerializeField] private int experienceToGive;      // Quantidade de XP que o jogador recebe ao derrotar este inimigo.

    [Header("Effects")]
    [SerializeField] private GameObject detroyEffect;   // Prefab do efeito visual (explosão, fumaça) a ser criado quando o inimigo morre.
    [SerializeField] private float pushTimer;           // Duração (em segundos) do efeito de empurrão (knockback) quando o inimigo leva dano.
    private float pushCounter;                          // Cronômetro interno para controlar o tempo restante do empurrão.

    private PlayerController player; // Referência ao script do jogador.
    private Vector2 direction;       // Vetor que armazena a direção do movimento.

    private void Awake()
    {
        // Pega a instância global do jogador.
        player = PlayerController.instance;

        // Verificações de segurança para garantir que as referências essenciais existem.
        if (player == null)
        {
            Debug.LogError("Enemy: PlayerController.instance está nulo.");
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // FixedUpdate é usado para lógica de física, garantindo que o movimento seja consistente.
    void FixedUpdate()
    {
        // Se o jogador não existe ou está morto, o inimigo para.
        if (player == null || !player.gameObject.activeSelf)
        {
            StopMovement();
            return;
        }

        // Executa as principais lógicas de comportamento do inimigo a cada passo da física.
        PushBack();
        MoveTowardsPlayer();
        FacePlayer();
    }

    // Controla o efeito de ser empurrado para trás (knockback).
    private void PushBack()
    {
        // Se o cronômetro de empurrão estiver ativo...
        if (pushCounter > 0)
        {
            pushCounter -= Time.deltaTime; // ...decrementa o tempo.

            // Inverte a velocidade para mover o inimigo para trás.
            // Esta verificação garante que a velocidade só seja invertida uma vez.
            if (speed > 0)
            {
                speed = -speed;
            }
            // Quando o tempo de empurrão acaba...
            if (pushCounter <= 0)
            {
                // ...restaura a velocidade ao seu valor original positivo usando Mathf.Abs.
                speed = Mathf.Abs(speed);
            }
        }
    }

    // Faz o sprite do inimigo virar para encarar o jogador.
    private void FacePlayer()
    {
        // spriteRenderer.flipX é um booleano. A expressão à direita retorna 'true' se o jogador
        // está à direita do inimigo (posição x do jogador > posição x do inimigo), fazendo o sprite virar.
        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    // Move o inimigo em direção ao jogador.
    private void MoveTowardsPlayer()
    {
        // Calcula o vetor de direção do inimigo para o jogador e o normaliza
        // (.normalized) para que a velocidade seja constante, independentemente da distância.
        direction = (player.transform.position - transform.position).normalized;
        // Aplica a velocidade ao corpo rígido na direção calculada.
        rb.linearVelocity = direction * speed;
    }

    // Para completamente o movimento do inimigo.
    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // Método da Unity chamado continuamente enquanto este objeto colide com outro.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Se o objeto tocado não for o jogador, não faz nada.
        if (!collision.gameObject.CompareTag("Player")) return;

        // Causa dano ao jogador.
        player.TakeDamage(damage);
    }

    // Método público chamado por armas do jogador para causar dano a este inimigo.
    public void TakeDamage(float damage)
    {
        health -= damage; // Reduz a vida.
        pushCounter = pushTimer; // Ativa o cronômetro de empurrão (knockback).

        // Chama um outro sistema para criar um número de dano flutuante na tela.
        DamageNumberController.instance.CreateNumber(damage, transform.position);

        // Verifica se a vida chegou a zero.
        if (health <= 0)
        {
            Destroy(gameObject); // Destrói o objeto do inimigo.
            Instantiate(detroyEffect, transform.position, transform.rotation); // Cria o efeito de morte.
            player.GetExperience(experienceToGive); // Concede experiência ao jogador.
        }
    }
}