using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private LayerMask PlayerMask;

    [Header("Position Values")]
    [SerializeField] private Transform searchPos;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float searchDistance;
    [SerializeField] private float attackDistance;
    RaycastHit2D searchLeft, searchRight, hitRight, hitLeft;
    private Transform playerFoundRight, playerFoundLeft, targetPlayer;

    [Header("Movement Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minMoveSpeed;

    private float baseMoveSpeed, speedToUpdateMoveSpeed;
    private bool followFlag, isBurning;

    [Header("Attack Values")]
    [SerializeField] private float attackDelay;

    private void Awake() {
        baseMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        targetPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!followFlag)
            Search();

        if (!isBurning)
            ResetMoveSpeed();
    }

    private void Search() {
        if (searchPos == null) {
            searchLeft = Physics2D.Raycast(transform.position, Vector2.left, searchDistance, PlayerMask);
            searchRight = Physics2D.Raycast(transform.position, Vector2.right, searchDistance, PlayerMask);
        } else {
            searchLeft = Physics2D.Raycast(searchPos.position, Vector2.left, searchDistance, PlayerMask);
            searchRight = Physics2D.Raycast(searchPos.position, Vector2.right, searchDistance, PlayerMask);
        }

        if (searchRight.collider != null) {
            if (!followFlag)
                followFlag = true;

            targetPlayer = searchRight.transform;
            
            /*
            playerFoundRight = searchRight.transform;
            if (playerFoundLeft == null)
                playerFoundLeft = playerFoundRight;

            // If new player found is closer than the target player, switch targets
            if (playerFoundRight.position.x - transform.position.x < targetPlayer.position.x - transform.position.x)
                targetPlayer = playerFoundRight;
            */
        }

        if (searchLeft.collider != null) {
            if (!followFlag)
                followFlag = true;

            targetPlayer = searchLeft.transform;

            /*
            playerFoundLeft = searchLeft.transform;
            if (playerFoundRight == null)
                playerFoundRight = playerFoundLeft;

            // If new player found is closer than the target player, switch targets
            if (playerFoundLeft.position.x - transform.position.x < targetPlayer.position.x - transform.position.x)
                targetPlayer = playerFoundLeft;
            */
        }
    }

    public float Movement() {
        if (!followFlag)
            return 0;

        // Follow the closest player
        //Vector2 direction = (targetPlayer.position - transform.position).normalized;
        //rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPlayer.position.x, transform.position.y), moveSpeed * Time.deltaTime);

        if (transform.position.x > targetPlayer.position.x)
            return -1;
        else if (transform.position.x <= targetPlayer.position.x)
            return 1;

        return 0;
    }

    public void ResetMoveSpeed() {
        if (isBurning)
            isBurning = false;

        if (moveSpeed < baseMoveSpeed) 
            moveSpeed += Time.deltaTime * speedToUpdateMoveSpeed;
        else if (moveSpeed > baseMoveSpeed) 
            moveSpeed = baseMoveSpeed;
    }

    public void SlowMoveSpeed(float maxBurnTime) {
        if (!isBurning)
            isBurning = true;

        speedToUpdateMoveSpeed = minMoveSpeed / maxBurnTime;

        if (moveSpeed > minMoveSpeed) 
            moveSpeed -= Time.deltaTime * speedToUpdateMoveSpeed;
        else if (moveSpeed < minMoveSpeed) 
            moveSpeed = minMoveSpeed;
    }

    public int Attack() {
        if (attackPos == null) {
            hitLeft = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, PlayerMask);
            hitRight = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, PlayerMask);
        } else {
            hitLeft = Physics2D.Raycast(attackPos.position, Vector2.left, attackDistance, PlayerMask);
            hitRight = Physics2D.Raycast(attackPos.position, Vector2.right, attackDistance, PlayerMask);
        }

        if (hitLeft.collider != null && hitRight.collider != null) 
            return (int)Random.Range(1, 2);
        else if (hitLeft.collider != null)
            return 1;
        else if (hitRight.collider != null)
            return 2;
        else if (hitRight.collider == null && hitLeft.collider == null)
            return 0;

        return 0;
    }

    public float GetAttackDelay() {
        return attackDelay;
    }
}
