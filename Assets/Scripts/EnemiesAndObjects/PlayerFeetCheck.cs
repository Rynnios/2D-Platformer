using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCheck : MonoBehaviour
{
    [SerializeField] SkullBoss skullBoss;


    private void Start()
    {
        skullBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<SkullBoss>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Kills normal enemies in 1 shot
        //If facing the boss, will reduce health of boss until 0, and then defeats the boss enemy
        if (collision.GetComponent<EnemyHeadCheck>())
        {
            skullBoss.bossHealthTracker();
            skullBoss.checkBossEnraged();
        }
    }
}
