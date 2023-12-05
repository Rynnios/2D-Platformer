using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCheck : MonoBehaviour
{
    public int bossHealth = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHeadCheck>())
        {
            if (GameObject.FindGameObjectWithTag("Boss"))
            {
                bossHealth -= 1;
            }
            else if (!GameObject.FindGameObjectWithTag("Boss") || bossHealth == 0)
            {
                killEnemy();
            }
        }
    }

    private void killEnemy()
    {
        Destroy(transform.parent.gameObject);
    }
}
