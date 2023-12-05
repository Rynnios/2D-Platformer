using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private float bounce = 20f;
    private bool bouncedOn = false;
    private enum MovementState { idle, inUse }   //idle has int value of 0

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bouncedOn = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            //UpdateAnimationState();
        }
        bouncedOn = false;
    }
    //private void UpdateAnimationState()
    //{
    //    MovementState state;

    //    if (bouncedOn)
    //    {
    //        state = MovementState.inUse;
    //    } 
    //    else if (!bouncedOn)
    //    {
    //        state = MovementState.idle;
    //    }

    //}
}
