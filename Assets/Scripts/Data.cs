using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    static public Data S; //Singleton

    //player fields
    public int skillPoints = 0;
    //public int level = 1;
    //public int playerLevel = 1;
    //public int enemyCount = 0;
    //public float attackDamage = 0.4f;
    //public float jumpForce = 5f;
    public int cooldownReducer = 0;
    public float moveSpeed = 7f;
    public float maxHealth = 3f;
    public float currentHealth = 3f;

    //skill tree fields
    public bool speedBtn1Disabled = false;
    public bool speedBtn2Disabled = false;
    public bool hpBtn1Disabled = false;
    public bool hpBtn2Disabled = false;
    public bool cooldownBtn1Disabled = false;
    public bool cooldownBtn2Disabled = false;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
            DontDestroyOnLoad(S);
        }

        else if (S != this)
        {
            Destroy(gameObject);
        }
    }

}