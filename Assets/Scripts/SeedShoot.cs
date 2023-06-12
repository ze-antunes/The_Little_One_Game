using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SeedShoot : MonoBehaviour
{
    // SerialPort sp;
    private Rigidbody2D rb;

    public float force;
    public float speed = 3;
    bool isColliding;

    float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * transform.localScale.x * speed;

        // sp = FindObjectOfType<ArduinoGameController>().sp; // set port of your arduino connected to computer
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 12) {
            Destroy(gameObject);
        }

        isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isColliding)
        {
            if (other.tag == "TilesetLayer")
            {
                Destroy(gameObject);
            }
            return;
        }
        isColliding = true;

        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerMovement>().TakeDamage(5);
            // sp.Write("H"); // Send 'H' character to the Arduino
            Destroy(gameObject);
        }
    }
}
