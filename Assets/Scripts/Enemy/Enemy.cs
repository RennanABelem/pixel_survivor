using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Stats")]
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float health;
    [SerializeField] private int experienceToGive;

    [Header("Effects")]
    [SerializeField] private GameObject detroyEffect;
    [SerializeField] private float pushTimer;
    private float pushCounter;

    private PlayerController player;
    private Vector2 direction;

    private void Awake()
    {
        player = PlayerController.instance;

        if (player == null)
        {
            Debug.LogError("Enemy: PlayerController.instance está nulo.");
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {

        if (player == null || !player.gameObject.activeSelf)
        {
            StopMovement();
            return;
        }

        PushBack();
        MoveTowardsPlayer();
        FacePlayer();
    }

    private void PushBack()
    {
        if(pushCounter > 0)
        {
            pushCounter -= Time.deltaTime;

            if(speed > 0)
            {
                speed = -speed;
            }
            if(pushCounter <= 0)
            {
                speed = Mathf.Abs(speed);
            }

        }
    }

    private void FacePlayer() {
        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    private void MoveTowardsPlayer() {

        direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
            player.TakeDamage(damage);
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        pushCounter = pushTimer;
        DamageNumberController.instance.CreateNumber(damage, transform.position);
        if(health <= 0)
        {
            Destroy(gameObject);
            Instantiate(detroyEffect, transform.position, transform.rotation);
            player.GetExperience(experienceToGive);
        }
    }

}
