using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmgTaken : MonoBehaviour
{
    public void hasEnemyTakenDmg(int bossHealth)
    {
        if (GameObject.FindGameObjectWithTag("Boss") && bossHealth != 0)
        {
            BossHealth(bossHealth);
            Debug.Log("Boss took 1 dmg!");

        }
        else if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            killEnemy();

        }
        else if (GameObject.FindGameObjectWithTag("Boss") || bossHealth == 0)
        {
            killEnemy();
        }
    }

    private void killEnemy()
    {
        Destroy(transform.parent.gameObject);
    }

    public void BossHealth(int bossHealth)
    {
        bossHealth--;
    }
}
