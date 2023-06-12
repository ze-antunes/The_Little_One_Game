using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI2 : MonoBehaviour
{
    // SerialPort sp;
    public Animator m_animator;

    //Reference to waypoints
    public List<Transform> points;

    //The int value for the next point index
    public int nextID = 0;

    //The value of that applies to ID for changing
    int idChangeValue = 1;

    // Movement Speed
    public float speed = 2;

    // Enemy health
    public int maxHealth = 100;
    public int currentHealth;

    public int damage;
    bool isColliding;

    void Start()
    {
        currentHealth = maxHealth;
        // sp = FindObjectOfType<ArduinoGameController>().sp; // set port of your arduino connected to computer
    }

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        // Make box collider trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        // Create Root object
        GameObject root = new GameObject(name + "_Root");
        // Reset Position of Root to enemy object
        root.transform.position = transform.position;
        // Set enemy object as child of root
        transform.SetParent(root.transform);
        // Create Waypoints object
        GameObject waypoints = new GameObject("Waypoints");
        // Reset waypoints position to root
        // Make waypoint object child of root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        // Create two points (gameObject) and reset their position to waypoints objects
        // Make the points children of waypoint object
        //P1
        GameObject p1 = new GameObject("P1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;
        //P2
        GameObject p2 = new GameObject("P2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        //Init points list then add the points to it
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();
        isColliding = false;
    }

    void MoveToNextPoint()
    {
        //Get tje next point transform
        Transform goalPoint = points[nextID];
        //Filp the enemy transform to look into the point's direction
        if (goalPoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }
        else
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }
        //Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(
            transform.position,
            goalPoint.position,
            speed * Time.deltaTime
        );
        //Check the distance between the enemy and goal point to trigger next point
        if (Vector2.Distance(transform.position, goalPoint.position) < 1f)
        {
            //Check if the player is at the end of the line (make the change -1)
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
            }
            //Check if the player is at the start of the line (make the change +1)
            if (nextID == 0)
            {
                idChangeValue = +1;
            }
            //Apply the change on the nextID
            nextID += idChangeValue;
        }
    }

    public void TakeDamage(int damage)
    {
        // yield return new WaitForSeconds(duration);
        currentHealth -= damage;

        // Play hurt animation
        m_animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Die animation
        m_animator.SetTrigger("Death");
        Destroy(this.GetComponent<Rigidbody2D>());
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding)
            return;
        isColliding = true;

        if (collision.tag == "Player")
        {
            Debug.Log("Hurt Player");
            FindObjectOfType<PlayerMovement>().TakeDamage(damage);
            // sp.Write("H"); // Send 'H' character to the Arduino
        }
    }
}
