using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WallCrawlerMovement : MonoBehaviour
{
    //Make crawler move right
    private Rigidbody2D crawlerRb;
    [SerializeField]
    private bool groundDetected;
    [SerializeField]
    private bool wallDetected;
    [SerializeField]
    private Transform groundPositionChecker;
    [SerializeField]
    private Transform wallPositionChecker;

    private float groundCheckerDistance = 0.2f;
    private float wallCheckerDistance = 0.1f;

    private bool hasTurned;

    private float zAxisAdd;
    private int direction;

    [SerializeField]
    private LayerMask whatIsGround;

    private void Start()
    {
        crawlerRb = GetComponent<Rigidbody2D>();
        hasTurned = false;
        direction = 1;
    }

    private void FixedUpdate()
    {
        CheckGroundOrWall();
        Movement();
    }

    void Movement()
    {
        crawlerRb.velocity = transform.right * 3;
    }

    void CheckGroundOrWall()
    {
        groundDetected = Physics2D.Raycast(groundPositionChecker.position, -transform.up, groundCheckerDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallPositionChecker.position, transform.right, wallCheckerDistance, whatIsGround);

        //if no ground is detected, rotate downwards 90 degrees
        if (!groundDetected)
        {
            if(hasTurned == false)
            {
                zAxisAdd -= 90;
                transform.eulerAngles = new Vector3(0, 0, zAxisAdd);

                if (direction == 1)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y);

                    hasTurned = true;
                    direction = 2;
                } 
                else if (direction == 2)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y);

                    hasTurned = true;
                    direction = 3;
                }
                else if (direction == 3)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y);

                    hasTurned = true;
                    direction = 4;
                }
                else if (direction == 4)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y);

                    hasTurned = true;
                    direction = 1;
                }
            }
        }
        if (groundDetected)
        {
            hasTurned = false;
        }

        if (wallDetected)
        {
            zAxisAdd += 90;
            transform.eulerAngles = new Vector3(0, 0, zAxisAdd);

            if (direction == 1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y);

                hasTurned = true;
                direction = 4;
            }
            else if (direction == 2)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y);

                hasTurned = true;
                direction = 1;
            }
            else if (direction == 3)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y);

                hasTurned = true;
                direction = 2;
            }
            else if (direction == 4)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y);

                hasTurned = true;
                direction = 3;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundPositionChecker.position, new Vector2(groundPositionChecker.position.x, groundPositionChecker.position.y - groundCheckerDistance));
        Gizmos.DrawLine(wallPositionChecker.position, new Vector2(wallPositionChecker.position.x + wallCheckerDistance, wallPositionChecker.position.y));
    }
}
