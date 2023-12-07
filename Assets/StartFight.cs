using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFight : MonoBehaviour
{
    private Animator bossAnim;
    SkullBoss skullBoss;

    private void Start()
    {
        bossAnim = GetComponent<Animator>();
        skullBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<SkullBoss>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            skullBoss.StartFight();
        }
    }
}
