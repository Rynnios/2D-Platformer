using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBoss : MonoBehaviour
{
    //Idle stage
    [Header("idle")]
    [SerializeField] float idleMoveSpeed;
    [SerializeField] Vector2 idleMoveDirection;

    [Header("Attack")]
    [SerializeField] float attackMoveSpeed;
    [SerializeField] Vector2 attackMoveDirection;

    [Header("TargetAttackPlayer")]
    [SerializeField] float followPlayerSpeed;
    [SerializeField] Transform player;
    private Vector2 playerPos;
    private bool hasPlayerPos;

    [Header("other")]
    [SerializeField] Transform WallUpCheck;
    [SerializeField] Transform WallDownCheck;
    [SerializeField] Transform WallSideCheck;
    [SerializeField] float wallCheckRadius;
    [SerializeField] LayerMask wallLayer;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingSide;

    private bool goingUp = true;
    private bool facingLeft = true;

    public int bossHealth = 5;

    private Rigidbody2D bossRb;
    private Animator bossAnim;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        idleMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        bossRb = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bossRb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(WallUpCheck.position, wallCheckRadius, wallLayer);
        isTouchingDown = Physics2D.OverlapCircle(WallDownCheck.position, wallCheckRadius, wallLayer);
        isTouchingSide = Physics2D.OverlapCircle(WallSideCheck.position, wallCheckRadius, wallLayer);
    }

    public void randomStatePicker()
    {
        int randomState = Random.Range(0, 2);

        if (randomState == 0)
        {
            //bounce attack
            bossAnim.SetTrigger("BounceAttack");
        }
        else if (randomState == 1)
        {
            //target attack
            bossAnim.SetTrigger("TargetAttack");

        }
    }

    public void IdleState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingSide)
        {
            Flip();
        }
        bossRb.velocity = idleMoveSpeed * idleMoveDirection;

    }

    public void EnragedAttackState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingSide)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        bossRb.velocity = attackMoveSpeed * attackMoveDirection;
    }

    public void TargetAttack()
    {
        if (!hasPlayerPos)
        {
            //Get player position
            playerPos = player.position - transform.position;

            //Normalize player position
            playerPos.Normalize();

            hasPlayerPos = true;
        }
        if (hasPlayerPos)
        {
            //Attack that position
            bossRb.velocity = playerPos * followPlayerSpeed;
        }

        if (isTouchingDown || isTouchingSide)
        {
            bossRb.velocity = Vector2.zero;
            hasPlayerPos = false;
            bossAnim.SetTrigger("Squish");
            StartCoroutine(FreezeBossRoutine(3.0f));
        }

    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idleMoveDirection.x *= -1;
        attackMoveDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idleMoveDirection.y *= -1;
        attackMoveDirection.y *= -1;
    }

    // Call this method to start the flash effect
    public void FlashDamageEffect()
    {
        StartCoroutine(DamageFlashCoroutine());
    }

    private IEnumerator DamageFlashCoroutine()
    {
        float flashDuration = 1.0f; // Total duration of the flash
        float flashInterval = 0.06f; // Time interval between flashes

        for (float timer = 0; timer < flashDuration; timer += flashInterval)
        {
            // Alternate between red and white color
            spriteRenderer.color = spriteRenderer.color == Color.white ? Color.red : Color.white;
            yield return new WaitForSeconds(flashInterval);
        }

        // Restore the original color (assuming it's white)
        spriteRenderer.color = Color.white;
    }

    private IEnumerator FreezeBossRoutine(float duration)
    {
        // Freeze the boss's movement and position
        bossRb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Unfreeze the boss's movement and position, but keep rotation frozen if needed
        bossRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(WallUpCheck.position, wallCheckRadius);
        Gizmos.DrawWireSphere(WallDownCheck.position, wallCheckRadius);
        Gizmos.DrawWireSphere(WallSideCheck.position, wallCheckRadius);

    }

    public void StartFight()
    {
        bossAnim.SetTrigger("startFight");
        bossRb.constraints = RigidbodyConstraints2D.None;
        bossRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void checkBossEnraged()
    {
        if (bossHealth <= 4)
        {
            bossAnim.SetTrigger("enraged");
            StartCoroutine(FreezeBossRoutine(2.6f));
        }
    }
}