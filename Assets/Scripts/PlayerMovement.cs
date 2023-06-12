using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // SerialPort sp;
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb;
    public float runSpeed = 40f;
    public float horizontalMove = 0f;
    public bool jump = false;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarScript healthBar;
    public int lifeCount = 1;
    public TextMeshProUGUI lifeCountText;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask emenyLayers;
    public int attackDamage = 20;
    public float attackRate = 3f;
    public float nextAttackTime = 0f;
    Vector3 startPos;
    public bool m_isDead = false;

    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;

    // Detection Radius
    public const float detectionRadius = .2f;

    // Detection Layer
    public LayerMask detectionLayer;

    //Cached Trigger Object
    public GameObject detectedObject;

    [Header("Sound Management")]
    public AudioSource jumpSoundEffect;

    void Start()
    {
        currentHealth = StateController.currentHealth;
        healthBar.SetHealthMax(maxHealth);
        healthBar.SetHealth(currentHealth);

        startPos = transform.position;
        lifeCountText.text = "x" + StateController.lives.ToString();

        if (
            SceneManager.GetActiveScene().buildIndex > 1
            && SceneManager.GetActiveScene().buildIndex < 5
        )
        {
            StateController.LevelPlayerPassed.Add(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("teste--------------------------------");
            string numbersAsString = string.Join(", ", StateController.LevelPlayerPassed);
            Debug.Log(numbersAsString);
        }
        // sp = FindObjectOfType<ArduinoGameController>().sp; // set port of your arduino connected to computer
    }

    private void Update()
    {
        if (!CanMove())
        {
            horizontalMove = 0;
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // if (sp.IsOpen)
        // {
        //     try
        //     {
        //         if (FindObjectOfType<ArduinoGameController>().testemove == -1)
        //         {
        //             horizontalMove = runSpeed * -1;
        //         }
        //         else if (FindObjectOfType<ArduinoGameController>().testemove == 1)
        //         {
        //             horizontalMove = runSpeed;
        //         }
        //         else
        //         {
        //             horizontalMove = 0;
        //         }
        //     }
        //     catch (System.Exception) { }
        // }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", jump);
            jumpSoundEffect.Play();
        }
        //Hurt
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // TakeDamage(20);
            FindObjectOfType<LevelLoader>()
                .LoadRandomLevel();
        }
        //Attack
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (Time.time >= nextAttackTime)
            {
                if (FindObjectOfType<Shooting>().canShoot)
                {
                    StartCoroutine(BowAttack(0f));
                    nextAttackTime = Time.time + 1.5f;
                }
                else
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }

        // Death
        if (currentHealth <= 0)
        {
            Debug.Log(lifeCount);
            Die();
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", jump);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    bool CanMove()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;
        if (FindObjectOfType<PauseMenu>().GameIsPaused)
            can = false;

        return can;
    }

    public void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEmenies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            emenyLayers
        );

        // Damage Them
        foreach (Collider2D emeny in hitEmenies)
        {
            // StartCoroutine(emeny.GetComponent<EnemyAI2>().TakeDamage(attackDamage, .5f));
            emeny.GetComponent<EnemyAI2>().TakeDamage(attackDamage);
        }
    }

    public IEnumerator BowAttack(float duration)
    {
        yield return new WaitForSeconds(duration);
        // Play an attack animation
        animator.SetTrigger("Attack");

        FindObjectOfType<Shooting>().CanShoot();
        FindObjectOfType<Shooting>().Shoot();
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        StateController.currentHealth = currentHealth;
    }

    void Die()
    {
        lifeCount--;
        StateController.lives = lifeCount;
        lifeCountText.text = "x" + lifeCount.ToString();
        // Debug.Log("Player died!");

        // Die animation
        if (lifeCount <= 0)
        {
            // animator.SetTrigger("Hurt");
            m_isDead = true;
            this.enabled = false;
            StartCoroutine(Respawn(1f));
        }
        else
        {
            healthBar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            StateController.currentHealth = currentHealth;
            lifeCountText.text = "x" + lifeCount.ToString();
        }
    }

    IEnumerator Respawn(float duration)
    {
        rb.simulated = false;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(duration);
        healthBar.SetHealth(maxHealth);
        currentHealth = maxHealth;
        lifeCount++;
        StateController.lives = lifeCount;
        StateController.currentHealth = currentHealth;
        lifeCountText.text = "x" + lifeCount.ToString();
        this.enabled = true;
        m_isDead = !m_isDead;
        transform.position = startPos;
        rb.simulated = true;
    }

    public bool DetectedEnemy()
    {
        Collider2D obj = Physics2D.OverlapCircle(
            detectionPoint.position,
            detectionRadius,
            detectionLayer
        );
        if (obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void GetExtraLife()
    {
        lifeCount++;
        StateController.lives = lifeCount;
        lifeCountText.text = "x" + lifeCount.ToString();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        Debug.Log("Game saved!!!!!");
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        StateController.currentHealth = data.health;
        Vector3 positionData;
        positionData.x = data.position[0];
        positionData.y = data.position[1];
        positionData.z = data.position[2];
        StartCoroutine(FindObjectOfType<MainMenu>().LoadLevel(data.level));
    }
}
