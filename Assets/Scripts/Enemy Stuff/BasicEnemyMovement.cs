using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    private SpriteRenderer spriteRend;

    //array of waypoints for objects to move back and forth from
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    //speed the moving platform goes
    [SerializeField] private float Speed = 2f;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.flipX = false;

    }
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
            spriteRend.flipX = !spriteRend.flipX;       //Makes enemy turn when reaches waypoint so it doesn't moonwalk

        }
        //Move object
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * Speed);

    }
}
