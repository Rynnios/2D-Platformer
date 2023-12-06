using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enraged_Run : StateMachineBehaviour
{
    public float speed = 7f;

    Transform player;
    Rigidbody2D rb;
    FinalBoss boss;

    public float minTime;
    public float maxTime;
    public float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Access player position
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<FinalBoss>();

        timer = Random.Range(minTime, maxTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();  //Boss turns towards player

        Vector2 target = new Vector2(player.position.x, rb.position.y); //Find target position to move to, since vertical (y-axis) is not needed, just keep it at bosses current y-axis
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);  //Move boss from current position to target position at a certain speed
        rb.MovePosition(newPos); 

        if (timer <= 0)
        {
            animator.SetTrigger("enragedJump");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
