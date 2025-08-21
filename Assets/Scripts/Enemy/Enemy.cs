using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Refer�ncia ao componente que renderiza o sprite, usado para virar o inimigo.
    [SerializeField] private Rigidbody2D rb;               // Refer�ncia ao corpo r�gido para controlar a f�sica e o movimento.

    [Header("Stats")]
    [SerializeField] private float speed;               // Velocidade de movimento do inimigo.
    [SerializeField] private int damage;                // Dano que o inimigo causa ao jogador por contato.
    [SerializeField] private float health;              // Pontos de vida do inimigo.
    [SerializeField] private int experienceToGive;      // Quantidade de XP que o jogador recebe ao derrotar este inimigo.

    [Header("Effects")]
    [SerializeField] private GameObject detroyEffect;   // Prefab do efeito visual (explos�o, fuma�a) a ser criado quando o inimigo morre.
    [SerializeField] private float pushTimer;           // Dura��o (em segundos) do efeito de empurr�o (knockback) quando o inimigo leva dano.
    private float pushCounter;                          // Cron�metro interno para controlar o tempo restante do empurr�o.

    private PlayerController player; // Refer�ncia ao script do jogador.
    private Vector2 direction;       // Vetor que armazena a dire��o do movimento.

    private void Awake()
    {
        // Pega a inst�ncia global do jogador.
        player = PlayerController.instance;

        // Verifica��es de seguran�a para garantir que as refer�ncias essenciais existem.
        if (player == null)
        {
            Debug.LogError("Enemy: PlayerController.instance est� nulo.");
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // FixedUpdate � usado para l�gica de f�sica, garantindo que o movimento seja consistente.
    void FixedUpdate()
    {
        // Se o jogador n�o existe ou est� morto, o inimigo para.
        if (player == null || !player.gameObject.activeSelf)
        {
            StopMovement();
            return;
        }

        // Executa as principais l�gicas de comportamento do inimigo a cada passo da f�sica.
        PushBack();
        MoveTowardsPlayer();
        FacePlayer();
    }

    // Controla o efeito de ser empurrado para tr�s (knockback).
    private void PushBack()
    {
        // Se o cron�metro de empurr�o estiver ativo...
        if (pushCounter > 0)
        {
            pushCounter -= Time.deltaTime; // ...decrementa o tempo.

            // Inverte a velocidade para mover o inimigo para tr�s.
            // Esta verifica��o garante que a velocidade s� seja invertida uma vez.
            if (speed > 0)
            {
                speed = -speed;
            }
            // Quando o tempo de empurr�o acaba...
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
        // spriteRenderer.flipX � um booleano. A express�o � direita retorna 'true' se o jogador
        // est� � direita do inimigo (posi��o x do jogador > posi��o x do inimigo), fazendo o sprite virar.
        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    // Move o inimigo em dire��o ao jogador.
    private void MoveTowardsPlayer()
    {
        // Calcula o vetor de dire��o do inimigo para o jogador e o normaliza
        // (.normalized) para que a velocidade seja constante, independentemente da dist�ncia.
        direction = (player.transform.position - transform.position).normalized;
        // Aplica a velocidade ao corpo r�gido na dire��o calculada.
        rb.linearVelocity = direction * speed;
    }

    // Para completamente o movimento do inimigo.
    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // M�todo da Unity chamado continuamente enquanto este objeto colide com outro.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Se o objeto tocado n�o for o jogador, n�o faz nada.
        if (!collision.gameObject.CompareTag("Player")) return;

        // Causa dano ao jogador.
        player.TakeDamage(damage);
    }

    // M�todo p�blico chamado por armas do jogador para causar dano a este inimigo.
    public void TakeDamage(float damage)
    {
        health -= damage; // Reduz a vida.
        pushCounter = pushTimer; // Ativa o cron�metro de empurr�o (knockback).

        // Chama um outro sistema para criar um n�mero de dano flutuante na tela.
        DamageNumberController.instance.CreateNumber(damage, transform.position);

        // Verifica se a vida chegou a zero.
        if (health <= 0)
        {
            Destroy(gameObject); // Destr�i o objeto do inimigo.
            Instantiate(detroyEffect, transform.position, transform.rotation); // Cria o efeito de morte.
            player.GetExperience(experienceToGive); // Concede experi�ncia ao jogador.
        }
    }
}