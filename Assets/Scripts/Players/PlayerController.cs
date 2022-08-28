using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerInput otherPlayerInput;
    [SerializeField] private float switchDelay;
    [SerializeField] private GameObject arrow;
    private bool switchReady = true;
    private bool activePlayer;

    [Header("Components")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerInput pi;
    private PlayerCombat pc;
    private PlayerMovement pm;
    private PlayerAnimation pa;
    private PlayerSFX ps;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Player AI")]
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private float maxDistanceToPlayer;

    [Header("Combat Values")]
    [SerializeField] private bool flashPlayer;
    [SerializeField] private bool meleePlayer;
    [SerializeField] private float attackDelay;
    [SerializeField] private float hurtInvincibleDelay;
    private float hurtAnimLength;
    private float attackAnimLength;
    private bool isDead, isHurt, isAttacking, isInvincible, attackReady;
    private bool secondaryAttack, secondaryAttackUp;
    private int gameState;

    void Awake() 
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (pi == null) pi = GetComponent<PlayerInput>();
        if (pm == null) pm = GetComponent<PlayerMovement>();
        if (pa == null) pa = GetComponent<PlayerAnimation>();
        if (pc == null) pc = GetComponent<PlayerCombat>();
        if (ps == null) ps = GetComponent<PlayerSFX>();
    
        if (meleePlayer) attackReady = true;
    }

    void Start() {
        gameState = GameManager._instance.GetGameState();

        hurtAnimLength = pa.GetAnimationLength(PlayerAnimHelper.PLAYER_HURT);
        attackAnimLength = pa.GetAnimationLength(PlayerAnimHelper.PLAYER_ATTACK);
        pi.SetGameState(gameState);
        if (flashPlayer)
            pi.SetPlayerType(1);
        else if (meleePlayer)
            pi.SetPlayerType(2);

        // If playing single player, turn melee character off initially
        if (gameState == 1) {
            if (meleePlayer)
                pi.enabled = false;
            else if (flashPlayer)
                activePlayer = true;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlayer();
        SetActiveVariables();
        ResetFlashPlayerAttack();
        if (!isDead && !isHurt && !isAttacking) {
            Movement();
            Animate();
        }
    }

    private void SetActiveVariables() {
        if (!pi.enabled && activePlayer) {
            activePlayer = false;
            arrow.SetActive(false);
        }
        else if (pi.enabled && !activePlayer) {
            activePlayer = true;
            arrow.SetActive(true);
        }
    }

    private void SwitchPlayer() {
        if (pi.GetSwitchCharPressed() && switchReady) {
            StartCoroutine(SwitchDelay());
        }
    }

    private void ResetFlashPlayerAttack() {
        if (flashPlayer && !otherPlayerInput.GetOtherPlayerAttackPressed() && isAttacking)
            isAttacking = false;
    }

    IEnumerator SwitchDelay() {
        switchReady = false;
        isAttacking = false;

        otherPlayerInput.enabled = true;
        pi.enabled = false;

        yield return new WaitForSeconds(switchDelay);
        switchReady = true;
    }

    private void Movement() {
        if (pi.GetAttackPressed() && flashPlayer)
            return;
        else if (isAttacking && meleePlayer)
            return;

        // AI CONTROLLED MOVEMENT
        if (!pi.enabled) {
            if (Mathf.Abs(transform.position.x - otherPlayerInput.transform.position.x) > maxDistanceToPlayer) 
                pm.AIMovement(otherPlayerInput.transform, runSpeed - 1f);
            else if (Mathf.Abs(transform.position.x - otherPlayerInput.transform.position.x) >= minDistanceToPlayer) 
                pm.AIMovement(otherPlayerInput.transform, walkSpeed - 0.5f);
            else if (meleePlayer && otherPlayerInput.GetOtherPlayerAttackPressed()) {
                // TODO PLAYER CONTROLLING AI MOVEMENT
                Debug.Log("Hello: " + otherPlayerInput.GetXAxis());
                if (!otherPlayerInput.GetRunInput())
                    pm.Movement(otherPlayerInput.GetXAxis(), walkSpeed);
                else
                    pm.Movement(otherPlayerInput.GetXAxis(), runSpeed);
            }
        }
        else if (!pi.GetRunInput())
            pm.Movement(pi.GetXAxis(), walkSpeed);
        else
            pm.Movement(pi.GetXAxis(), runSpeed);
    }

    private void Animate() {
        // AI Controlled Behavior
        if (!pi.enabled) {
            if (Mathf.Abs(transform.position.x - otherPlayerInput.transform.position.x) > maxDistanceToPlayer) 
                pa.RunAnim();
            else if (Mathf.Abs(transform.position.x - otherPlayerInput.transform.position.x) > minDistanceToPlayer) 
                pa.WalkAnim();
            else if (Mathf.Abs(transform.position.x - otherPlayerInput.transform.position.x) <= minDistanceToPlayer) 
                pa.IdleAnim();
            
            // Face other player direction
            if (otherPlayerInput.transform.position.x < transform.position.x)
                transform.localScale = new Vector2(-1f, transform.localScale.y);
            else if (otherPlayerInput.transform.position.x >= transform.position.x)
                transform.localScale = new Vector2(1f, transform.localScale.y);

            // Attack
            if (flashPlayer) {
                if (otherPlayerInput.GetOtherPlayerAttackPressed() && pi.GetYAxis() > 0) {
                    isAttacking = true;
                    AIAttackDirection();
                    pa.AttackUpAnim();
                }
                else if (otherPlayerInput.GetOtherPlayerAttackPressed()) {
                    isAttacking = true;
                    AIAttackDirection();
                    pa.AttackAnim();
                }
            } 
            else if (meleePlayer) {
                if (otherPlayerInput.GetOtherPlayerAttackPressed() && attackReady) {
                    AIAttackDirection();
                    StartCoroutine(MeleeAttack());
                }
            }

            return;
        }

        if (flashPlayer) {
            if (pi.GetAttackPressed() && pi.GetYAxis() > 0) 
                pa.AttackUpAnim();
            else if (pi.GetAttackPressed()) {
                pa.AttackAnim();
            }
            else if (pi.GetXAxis() == 0)
                pa.IdleAnim();
            else if (pi.GetXAxis() != 0 && !pi.GetRunInput()) {
                pa.WalkAnim();

                // Face the direction player is walking
                if (pi.GetXAxis() > 0)
                    transform.localScale = new Vector2(1f, transform.localScale.y);
                else if (pi.GetXAxis() < 0)
                    transform.localScale = new Vector2(-1f, transform.localScale.y);
            }
            else if (pi.GetXAxis() != 0 && pi.GetRunInput()) {
                pa.RunAnim();

                // Face the direction player is walking
                if (pi.GetXAxis() > 0)
                    transform.localScale = new Vector2(1f, transform.localScale.y);
                else if (pi.GetXAxis() < 0)
                    transform.localScale = new Vector2(-1f, transform.localScale.y);
            }
        } else if (meleePlayer) {
            if (pi.GetAttackPressed() && attackReady) 
                StartCoroutine(MeleeAttack());
            else if (pi.GetXAxis() == 0)
                pa.IdleAnim();
            else if (pi.GetXAxis() != 0 && !pi.GetRunInput()) {
                pa.WalkAnim();

                // Face the direction player is walking
                if (pi.GetXAxis() > 0)
                    transform.localScale = new Vector2(1f, transform.localScale.y);
                else if (pi.GetXAxis() < 0)
                    transform.localScale = new Vector2(-1f, transform.localScale.y);
            }
            else if (pi.GetXAxis() != 0 && pi.GetRunInput()) {
                pa.RunAnim();

                // Face the direction player is walking
                if (pi.GetXAxis() > 0)
                    transform.localScale = new Vector2(1f, transform.localScale.y);
                else if (pi.GetXAxis() < 0)
                    transform.localScale = new Vector2(-1f, transform.localScale.y);
            }
        }
    }

    private void AIAttackDirection() {
        if (pc.AIAttack() == 1)
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        else if (pc.AIAttack() == 2)
            transform.localScale = new Vector2(1f, transform.localScale.y);
    }

    IEnumerator MeleeAttack() {
        pa.AttackAnim();
        isAttacking = true;
        attackReady = false;
        ps.PlayAttackSFX();
        yield return new WaitForSeconds(attackAnimLength);

        isAttacking = false;
        yield return new WaitForSeconds(attackDelay);
        attackReady = true;
    }

    public void TakeDamage(int damage) {
        if (isInvincible || isDead)
            return;

        ps.PlayHurtSFX();
        pc.TakeDamage(damage);
        if (pc.GetCurrentHealth() <= 0)
            Death();
        else
            StartCoroutine(Hurt());
    }

    IEnumerator Hurt() {
        isHurt = true;
        isInvincible = true;
        pa.HurtAnim();
        yield return new WaitForSeconds(hurtAnimLength + 0.15f);

        isHurt = false;
        yield return new WaitForSeconds(hurtInvincibleDelay);
        isInvincible = false;
    }

    private void Death() {
        isDead = true;
        pa.DeathAnim();
        ps.PlayDeathSFX();
        pc.enabled = false;
        pi.enabled = false;
        ps.enabled = false;
    }

    public bool GetActivePlayer() {
        return activePlayer;
    }

    public bool GetIsDead() {
        return isDead;
    }
}
