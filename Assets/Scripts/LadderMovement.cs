using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    public float speed = 8f;
    public bool isLadder;
    public bool isClimbing;

    [SerializeField]
    private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
            FindObjectOfType<PlayerMovement>().animator.SetBool("isClimbing", true);
            FindObjectOfType<PlayerMovement>().animator.SetBool("IsJumping", false);
            FindObjectOfType<PlayerMovement>().jump = false;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            FindObjectOfType<PlayerMovement>().jump = false;
            FindObjectOfType<PlayerMovement>().animator.SetBool("isClimbing", true);   
            FindObjectOfType<PlayerMovement>().animator.SetBool("IsJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            FindObjectOfType<PlayerMovement>().animator.SetBool("isClimbing", false);
        }
    }
}
