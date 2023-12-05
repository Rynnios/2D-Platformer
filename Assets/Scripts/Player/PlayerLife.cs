using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    // Don't know why it's spelt "Sigleton"
    #region Sigleton 
    private static PlayerLife instance;
    public static PlayerLife Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerLife>();
            return instance;
        }
    }
    #endregion

    public float currentHealth;
    public float maxHealth;
    public float maxTotalHealth;

    public float Health { get { return currentHealth; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    static public PlayerLife S; // Singleton

    private void Awake()
    {
        S = this; //setting Singleton
    }

    // Start is called before the first frame update
    private void Start()
    {
        maxHealth = Data.S.maxHealth;
        currentHealth = Data.S.currentHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    
    public void Heal(float health)
    {
        this.currentHealth += health;
        ClampHealth();
        Data.S.currentHealth = currentHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        ClampHealth();
        Data.S.currentHealth = currentHealth;
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            currentHealth = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
        Data.S.currentHealth = currentHealth;
    }

    void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("FallOffScreen"))
        {
            PlayerDies();
        }

        else if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1.0f);
            if (currentHealth == 0f)
            {
                PlayerDies();
            }
        }
    }

    private void PlayerDies()
    {
        animator.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
