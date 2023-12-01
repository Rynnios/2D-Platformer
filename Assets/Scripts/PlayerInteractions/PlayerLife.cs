using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public int skillPoints = 0;

    static public PlayerLife S; //singleton

    private void Awake()
    {
        S = this; //setting Singleton
    }

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skillPoints = Data.S.skillPoints;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("FallOffScreen"))
        {
            PlayerDies();
        }
    }

    private void PlayerDies()
    {
        animator.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void SetText()
    //{
        //killTree.S.skillText.text = SkillTree.S.skillText.text + skillPoints.ToString();
    //}
}
