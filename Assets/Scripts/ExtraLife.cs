using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ExtraLife : MonoBehaviour
{
    bool isColliding;

    //Collider Trigger
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 8;
    }

    void Update()
    {
        isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding)
            return;
        isColliding = true;

        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<PlayerMovement>().GetExtraLife();
            Destroy(gameObject);
        }
    }
}
