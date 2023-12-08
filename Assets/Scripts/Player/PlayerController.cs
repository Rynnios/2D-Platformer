using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRend;
    private Animator animator;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    public float currentHealth = 3;
    public float maxHealth = 3;
    public float maxTotalHealth = 8;
    public bool isInvincible;
    public bool isControlEnabled = true;
    private bool canDoubleJump = false;
    private bool doubleJumpUsed = false;

    public AudioSource deathSound;
    public AudioSource damageSound;
    public AudioSource jumpSound;

    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;


    private enum MovementState { idle, running, jumping, falling }   // idle has int value of 0
    static public PlayerController Instance { get; private set; } // Use private set for singleton

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        maxHealth = Data.S.maxHealth;
        currentHealth = maxHealth;
        onHealthChangedCallback?.Invoke(); // Force health bar update

        Physics.IgnoreLayerCollision(0,8);
    }

    private void Update()
    {
        if (!isControlEnabled) return;

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * Data.S.moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (OnGround())
            {
                Jump();
                canDoubleJump = Data.S.doubleJumpEnabled; // Allow double jump if it's enabled (SMART!!!)
            }
            else if (canDoubleJump && !doubleJumpUsed)
            {
                DoubleJump();
            }
        }

        UpdateAnimationState();
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * Data.S.jumpPower, ForceMode2D.Impulse);
        jumpSound.Play();
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y velocity
        rb.AddForce(Vector2.up * 11f, ForceMode2D.Impulse);
        jumpSound.Play();
        animator.SetTrigger("doubleJump"); // Make sure to set up a "doubleJump" trigger in your Animator
        doubleJumpUsed = true; // Prevent further jumps until landing
    }

    private void UpdateAnimationState()
    {
        // MovementState is an existing enum you have that defines the player's current movement status.
        MovementState state;

        // Flip sprite based on horizontal movement
        if (dirX > 0f)
        {
            spriteRend.flipX = false;
        }
        else if (dirX < -0f)
        {
            spriteRend.flipX = true;
        }

        // Check if the player is on the ground.
        if (OnGround())
        {
            // Reset the double jump flags when the player has landed.
            canDoubleJump = Data.S.doubleJumpEnabled;
            doubleJumpUsed = false;

            // Determine the movement state based on the Rigidbody's horizontal velocity.
            if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon) // Mathf.Epsilon is a very small number, effectively zero.
            {
                state = MovementState.running;
            }
            else
            {
                state = MovementState.idle;
            }
        }
        else // If not on the ground
        {
            // Determine if the player is jumping or falling based on vertical velocity
            if (rb.velocity.y > 0.1f)
            {
                state = MovementState.jumping;
            }
            else
            {
                state = MovementState.falling;
            }
        }

        // Set the animation state in the Animator.
        animator.SetInteger("state", (int)state);
    }

    private bool OnGround()
    {
        //BoxCast basically creates a box around the player that has the same shape as the box collider through the first 2 arguments
        //0f is the rotation
        //Vector2.down by .1f moves box down slightly downwards
        //will return true or false depending if player is touching on the ground or not

        // Reduce the height of the box to cover only the lower half of the player
        Vector2 size = new Vector2(boxCollider.bounds.size.x * 0.99f, boxCollider.bounds.size.y * 0.5f);

        return Physics2D.BoxCast(boxCollider.bounds.center, size, 0f, Vector2.down, .5f, jumpableGround);
    }

    public void Heal(float health)
    {
        this.currentHealth += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg, Vector2 enemyPosition)
    {
        if (!isInvincible)
        {
            damageSound.Play();
            currentHealth -= dmg;
            ClampHealth();
            
            // Knockback if not dead
            if (currentHealth > 0)
            {
                // Determine knockback direction based on enemy position relative to player
                Vector2 knockbackDirection = transform.position.x > enemyPosition.x ? Vector2.right : Vector2.left;

                // Set the player's velocity to a specific value for consistent knockback
                rb.velocity = new Vector2(knockbackDirection.x * Data.S.knockbackStrengthX, Data.S.knockbackStrengthY);

                // Disable player control temporarily
                isControlEnabled = false;

                animator.SetTrigger("hurt");

                // Start coroutine to enable control after landing
                StartCoroutine(WaitForLanding());
            }
        }
    }

    public void TakeDamageWithImmediateIFrames(float dmg, Vector2 enemyPosition)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityFrames(3.5f));
            damageSound.Play();
            currentHealth -= dmg;
            ClampHealth();

            // Knockback if not dead
            if (currentHealth > 0)
            {
                // Determine knockback direction based on enemy position relative to player
                Vector2 knockbackDirection = transform.position.x > enemyPosition.x ? Vector2.right : Vector2.left;

                // Set the player's velocity to a specific value for consistent knockback
                rb.velocity = new Vector2(knockbackDirection.x * Data.S.knockbackStrengthX, Data.S.knockbackStrengthY);

                // Disable player control temporarily
                isControlEnabled = false;

                animator.SetTrigger("hurt");

                // Start coroutine to enable control after landing
                StartCoroutine(WaitForLandingNoIFrames());
            }
        }
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
        Data.S.maxHealth = maxHealth;
    }

    void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && !isInvincible && isControlEnabled)
        {
            // Kill enemy if true invincible
            if (Data.S.trueInvincibilityEnabled)
            {
                PlayerFeetCheck feetCheck = collider.GetComponentInChildren<PlayerFeetCheck>();

                if (feetCheck != null)
                {
                    feetCheck.DefeatEnemy();
                }
            }
            else
            {
                // Use the enemy's position directly for knockback calculation
                TakeDamage(1.0f, collider.transform.position);
                if (currentHealth <= 0)
                {
                    PlayerDies();
                }
            }
        }
        else if (collider.gameObject.CompareTag("Trap") && !Data.S.trueInvincibilityEnabled)
        {
            currentHealth = 0;
            onHealthChangedCallback?.Invoke(); // Force health bar update
            PlayerDies();
        }
        else if (collider.gameObject.CompareTag("Boss"))
        {
            // Use the enemy's position directly for knockback calculation
            TakeDamageWithImmediateIFrames(1.0f, collider.transform.position);
            if (currentHealth <= 0)
            {
                PlayerDies();
            }
        }
    }

    private void PlayerDies()
    {
        deathSound.Play();
        animator.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;

        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay() // Delay death so animation can play out a bit longer
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !OnGround());
        yield return new WaitUntil(() => OnGround());

        // Reset hurt animation trigger
        animator.ResetTrigger("hurt");
        animator.SetInteger("state", (int)MovementState.idle);

        // Re-enable player control and start invincibility frames
        isControlEnabled = true;
        StartCoroutine(InvincibilityFrames(Data.S.invincibilityDuration));
    }

    IEnumerator WaitForLandingNoIFrames()
    {
        yield return new WaitUntil(() => !OnGround());
        yield return new WaitUntil(() => OnGround());

        // Reset hurt animation trigger
        animator.ResetTrigger("hurt");
        animator.SetInteger("state", (int)MovementState.idle);

        // Re-enable player control and start invincibility frames
        isControlEnabled = true;
    }

    IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < duration)
        {
            // Oscillate alpha between 70 and 170 using a sine wave
            float alpha = (Mathf.Sin(timer * 10f) + 1f) * 0.5f; // This oscillates between 0 and 1
            alpha = Mathf.Lerp(70f / 255f, 170f / 255f, alpha); // Map to range between 70 and 170
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        // Reset sprite to full opacity
        spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
        isInvincible = false;
    }
}