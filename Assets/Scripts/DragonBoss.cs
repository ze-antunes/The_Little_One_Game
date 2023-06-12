using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public Animator animator;

    // Enemy health
    public int maxHealth = 200;
    public int currentHealth;
    public HealthBarScript healthBar;

    float timer;

    public GameObject dragonMedal;
    
    [Header("Sound Management")]
    public AudioSource medalSoundEffect;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealthMax(maxHealth);
    }

    void Update(){
        timer += Time.deltaTime;

        if (timer > 3)
        {
            timer = 0;
            StartCoroutine(Attack(.5f));
        }

        if(FindObjectOfType<PlayerMovement>().lifeCount <= 0){
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
    }

    public IEnumerator Attack(float duration){
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(duration);
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
    
    public void TakeDamage(int damage){

        // yield return new WaitForSeconds(duration);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Play hurt animation 
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0){
            Die();
        }
        Debug.Log("Dragon TakeDamage");
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Die animation 
        animator.SetTrigger("Death");
        Destroy(this.GetComponent<Rigidbody2D>());
        GetComponent<PolygonCollider2D>().enabled = false;
        this.enabled = false;
        medalSoundEffect.Play();
        FindObjectOfType<InventorySystem>().Medal(dragonMedal);
        FindObjectOfType<PlayerMovement>().SavePlayer();
    }
}
