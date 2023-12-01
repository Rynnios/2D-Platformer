using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private SpriteRenderer spriteRend;
    private Animator animator;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;      //SerializeField hides from other scripts but allows you to modify values in unity
    [SerializeField] private float jumpStrength = 25f;
    private enum MovementState { idle, running, jumping, falling}   //idle has int value of 0

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");        //GetAxisRaw makes it so value doesn't go to 0 immediately after releasing button
        rb.velocity = new Vector2 (dirX * moveSpeed, rb.velocity.y);
        

        if (Input.GetButtonDown("Jump") && OnGround() )
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x, jumpStrength);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            spriteRend.flipX = false;
        }
        else if (dirX < 0)
        {
            state = MovementState.running;
            spriteRend.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.2f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool OnGround()
    {
        //BoxCast basically creates a box around the player that has the same shape as the box collider through the first 2 arguments
        //0f is the rotation
        //Vector2.down by .1f moves box down slightly downwards
        //will return true or false depending if player is touching on the ground or not
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
