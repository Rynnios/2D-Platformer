using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    public int health;
    public int dmg;
    private float timeBtwnDmg = 1.5f;

    public Animator redPanel;
    public Animator camAnim;

    public Slider bossHealthBar;

    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player"))
    }
}
