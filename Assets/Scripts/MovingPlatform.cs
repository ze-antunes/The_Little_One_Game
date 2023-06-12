using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoint;
    public float moveSpeed = 2;

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        //Change position of the platform (move to the goal point)
        platform.position = Vector2.MoveTowards(
            platform.position,
            points[goalPoint].position,
            1 * Time.deltaTime * moveSpeed
        );
        //Check if we are close proximitu of the next point
        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            //If so change goal to the next one
            //Check if we reached the last point, reset to first point
            if(goalPoint == points.Count -1){
                goalPoint = 0;
            } else {
                goalPoint++;
            }
        }
    }
}
