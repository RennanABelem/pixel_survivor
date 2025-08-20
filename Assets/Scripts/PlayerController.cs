using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    private UIController ui;

    [Header("Stats")]
    [SerializeField] private int speed;
    [SerializeField] public float maxHealth; 
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;

    public float currentHealth;
    private bool isImune;
    private Vector2 movement;

    [Header("XP system")]
    public int currentLevel;
    public int maxLevel;
    public int currentExperience;
    public List<int> playerLevels;

    public Weapon activeWeapon;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ui = UIController.instance;
        currentHealth = maxHealth;

        ui?.UpdateHealthSlider();
    }

    private void Update()
    {
        HandleImmunityDuration();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleImmunityDuration() {
        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            isImune = false;
        }
    }

    public void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
        UpdateAnimator(movement);
    }

    private void Move()
    {
        rb.linearVelocity = movement.normalized * speed;
    }

    private void UpdateAnimator(Vector2 movement)
    {
        //recuperando os valores para o animator trabalhar a movimentacao dos sprites.
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        //verificando se o elemento esta parado e retornando para o animator 
        animator.SetBool("isMoving", movement != Vector2.zero);

    }

    public void TakeDamage(float damage)
    {
        if (isImune || damage <= 0) return;
        
        isImune = true;
        immunityTimer = immunityDuration;

        currentHealth -= damage;
        ui?.UpdateHealthSlider();
        
        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.instance.GameOver();
    }

    public void GetExperience(int experienceToGet)
    {
        currentExperience += experienceToGet;
        ui.UpdateExperienceSlider();

        if(currentExperience >= playerLevels[currentLevel])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentExperience -= playerLevels[currentLevel];
        currentLevel++;
        ui.UpdateExperienceSlider();
        ui.levelUpButtons[0].ActivateButton(activeWeapon);
        ui.LevelUpPanelOpen();
    }
}
