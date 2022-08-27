using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAI : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private LayerMask PlayerMask;

    [Header("Movement Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayDistance;

    private bool followFlag;
    private Transform playerFoundRight, playerFoundLeft, targetPlayer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, PlayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, PlayerMask);

        if (hitRight.collider != null) {
            if (!followFlag)
                followFlag = true;

            playerFoundRight = hitRight.transform;
            if (playerFoundLeft == null)
                playerFoundLeft = playerFoundRight;

            // If new player found is closer than the target player, switch targets
            if (playerFoundRight.position.x - transform.position.x < targetPlayer.position.x - transform.position.x)
                targetPlayer = playerFoundRight;
        }

        if (hitLeft.collider != null) {
            if (!followFlag)
                followFlag = true;

            playerFoundLeft = hitLeft.transform;
            if (playerFoundRight == null)
                playerFoundRight = playerFoundLeft;

            // If new player found is closer than the target player, switch targets
            if (playerFoundLeft.position.x - transform.position.x < targetPlayer.position.x - transform.position.x)
                targetPlayer = playerFoundLeft;
        }
    }

    public void Movement() {
        if (!followFlag)
            return;

        // Follow the closest player
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
