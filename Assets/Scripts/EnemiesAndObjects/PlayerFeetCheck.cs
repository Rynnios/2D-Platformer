using UnityEngine;

public class PlayerFeetCheck : MonoBehaviour
{
    public AudioSource enemyKillSound;
    private SkullBoss skullBoss;

    public void Start()
    {
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            skullBoss = bossObject.GetComponent<SkullBoss>();
        }
    }

    public void Die(GameObject enemy)
    {
        // Flip sprite upside down and bring forward
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipY = true;
            spriteRenderer.sortingOrder += 1; // Increase sorting order to bring sprite forward
        }

        // Add Rigidbody2D if not present and apply settings
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = enemy.AddComponent<Rigidbody2D>();
        }

        // Apply an initial upward force and stop horizontal movement
        rb.velocity = new Vector2(0, Vector2.up.y * 10f); // Increased force, stopped horizontal movement

        // Increase gravity scale to make it fall faster
        rb.gravityScale = 3.0f;

        // Destroy the GameObject after 3 seconds
        Destroy(enemy, 3f);
    }

    public void DefeatEnemy()
    {
        GameObject enemy = transform.parent.gameObject;
        enemyKillSound.Play();

        // Damage the boss if there's a boss
        if (skullBoss)
        {
            skullBoss.bossHealth--;
            if (skullBoss.bossHealth > 0)
            {
                skullBoss.FlashDamageEffect(); // Call the flash effect method
                skullBoss.checkBossEnraged();
            }
            else
            {
                Finish finishScript = FindObjectOfType<Finish>(); // Find the Finish script in the scene
                if (finishScript != null)
                {
                    finishScript.BossDefeated(); // Call the function for when the boss is defeated
                }


                Destroy(skullBoss);

                Rigidbody2D bossRb = enemy.GetComponent<Rigidbody2D>();
                if (bossRb != null)
                {
                    bossRb.velocity = Vector2.zero; // Stop any ongoing movement
                    bossRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Unfreeze the boss's movement and position, but keep rotation frozen if needed
                }

                var bossAnimator = enemy.GetComponent<Animator>();
                if (bossAnimator != null)
                {
                    Destroy(bossAnimator);
                }

                var bossCollider = enemy.GetComponent<CapsuleCollider2D>();
                if (bossCollider != null)
                {
                    Destroy(bossCollider);
                }

                // Clear all children
                foreach (Transform child in enemy.transform)
                {
                    Destroy(child.gameObject);
                }

                Die(GameObject.FindGameObjectWithTag("Boss"));
            }
        }
        else
        {
            // Check for and disable BasicEnemyMovement script
            BasicEnemyMovement enemyMovement = enemy.GetComponent<BasicEnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.enabled = false;
            }

            // Remove PlatformEffector2D and BoxCollider2D components if they exist
            var platformEffector = enemy.GetComponent<PlatformEffector2D>();
            if (platformEffector != null)
            {
                Destroy(platformEffector);
            }

            var boxCollider = enemy.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                Destroy(boxCollider);
            }

            // Clear all children
            foreach (Transform child in enemy.transform)
            {
                Destroy(child.gameObject);
            }

            Die(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHeadCheck>())
        {
            DefeatEnemy();
        }
    }
}