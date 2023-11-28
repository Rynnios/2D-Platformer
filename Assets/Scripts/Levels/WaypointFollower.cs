using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    //array of waypoints to 
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    //speed that platform moves
    [SerializeField] private float Speed = 2f;

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f) //if current waypoint is smaller than num, it is touching waypoint and switch
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        //Move platform
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * Speed);

    }
}
