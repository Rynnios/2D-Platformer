using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Behaviour : StateMachineBehaviour
{
    public float speed = 3f;

    Transform player;
    Rigidbody2D rb;
    FinalBoss boss;

    public PlayerFeetCheck playerFeetCheck;
    public int bossHealth;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Access player position
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<FinalBoss>();

        //playerFeetCheck = GameObject.Find("PlayerFeetCheck").GetComponent<PlayerFeetCheck>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();  //Boss turns towards player

        Vector2 target = new Vector2(player.position.x, rb.position.y); //Find target position to move to, since vertical (y-axis) is not needed, just keep it at bosses current y-axis
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);  //Move boss from current position to target position at a certain speed
        rb.MovePosition(newPos);  //Update position of Boss Pig

    }


}
