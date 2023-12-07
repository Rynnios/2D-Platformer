using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCheck : MonoBehaviour
{
    public int bossHealth = 5;

    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Kills normal enemies in 1 shot
        //If facing the boss, will reduce health of boss until 0, and then defeats the boss enemy
        if (collision.GetComponent<EnemyHeadCheck>())
        {
            if (GameObject.FindGameObjectWithTag("Boss") && bossHealth != 0)
            {
                BossHealth();
            }
            else if (GameObject.FindGameObjectWithTag("Enemy"))
            {
                killEnemy();

            } else if (GameObject.FindGameObjectWithTag("Boss") || bossHealth == 0)
            {
                killEnemy();
            }
        }
    }

    private void Update()
    {
        if (bossHealth <= 4)
        {
            animator.SetTrigger("turnEnraged");
            Debug.Log("Boss health is now 4!");
        }
    }

    private void killEnemy()
    {
        Destroy(transform.parent.gameObject);
    }

    public void BossHealth()
    {
        bossHealth--;

    }
}
