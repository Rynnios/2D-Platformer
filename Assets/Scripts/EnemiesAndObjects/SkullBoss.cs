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
    public Collider2D collision;

    private Rigidbody2D bossRb;
    private Animator bossAnim;

    // Start is called before the first frame update
    void Start()
    {
        idleMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        bossRb = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();

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
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
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
        if (bossHealth <= 3)
        {
            bossAnim.SetTrigger("enraged");
            Debug.Log("Boss health is now 4!");
        }
    }

    public void bossHealthTracker()
    {
        if (bossHealth != 0)
        {
            bossHealth--;
        }
        else if (bossHealth == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
