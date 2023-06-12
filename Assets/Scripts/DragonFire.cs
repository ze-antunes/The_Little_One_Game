using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class DragonFire : MonoBehaviour
{
    // SerialPort sp;
    private GameObject player;
    private Rigidbody2D rb;

    public float force;
    bool isColliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float bulletRotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, bulletRotation);

        // sp = FindObjectOfType<ArduinoGameController>().sp; // set port of your arduino connected to computer
    }

    void Update()
    {
        isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isColliding)
        {
            if (other.tag == "FireBlocker")
            {
                Destroy(gameObject);
            }
            return;
        }
        isColliding = true;

        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerMovement>().TakeDamage(15);
            // sp.Write("H"); // Send 'H' character to the Arduino
            Destroy(gameObject);
        }
    }

}
