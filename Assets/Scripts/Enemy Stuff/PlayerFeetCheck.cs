using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCheck : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHeadCheck>())
        {
            enemyDeath();
        }
    }

    private void enemyDeath()
    {
        animator.SetTrigger("death");
        Destroy(transform.parent.gameObject);
    }
}
