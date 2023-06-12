using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EmenyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform emenyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPahth = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPahth = true;
            return;
        }
        else
        {
            reachedEndOfPahth = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        rb.AddForce(force);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(force.x >= 0.01f){
            emenyGFX.localScale = new Vector3(-5.5f, 5.5f, 5.5f);
        } else if(force.x <= 0.01f){
            emenyGFX.localScale = new Vector3(5.5f, 5.5f, 5.5f);
        }
    }
}
