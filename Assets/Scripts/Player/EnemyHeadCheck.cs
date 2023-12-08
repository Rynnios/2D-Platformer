using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadCheck : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRb;
    [SerializeField]
    private PlayerController playerController;
    public float upForce = 600f;
    public float sideForce = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFeetCheck>())
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
            playerRb.AddForce(Vector2.up * upForce);
            if (sideForce > 0f)
            {
                StartCoroutine(IgnoreInput(0.8f));
            }
        }

    }

    private IEnumerator IgnoreInput(float duration)
    {
        playerController.isControlEnabled = false;
        playerRb.velocity = new Vector2(sideForce, playerRb.velocity.y);
        yield return new WaitForSeconds(duration);
        playerController.isControlEnabled = true;
    }
}
