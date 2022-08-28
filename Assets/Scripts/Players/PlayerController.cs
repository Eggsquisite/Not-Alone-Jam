using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerInput otherPlayerInput;
    [SerializeField] private float switchDelay;
    private bool switchReady = true;
    private bool mainCharacter;

    [Header("Components")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerInput pi;
    private PlayerCombat pc;
    private PlayerMovement pm;
    private PlayerAnimation pa;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Combat Values")]
    [SerializeField] private bool flashPlayer;
    [SerializeField] private bool meleePlayer;
    [SerializeField] private float attackDelay;
    [SerializeField] private float hurtInvincibleDelay;
    private float hurtAnimLength;
    private float attackAnimLength;
    private bool isDead, isHurt, isAttacking, isInvincible, attackReady;
    private int gameState;

    void Awake() 
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (pi == null) pi = GetComponent<PlayerInput>();
        if (pm == null) pm = GetComponent<PlayerMovement>();
        if (pa == null) pa = GetComponent<PlayerAnimation>();
        if (pc == null) pc = GetComponent<PlayerCombat>();

        if (meleePlayer) attackReady = true;
        hurtAnimLength = pa.GetAnimationLength(PlayerAnimHelper.PLAYER_HURT);
        attackAnimLength = pa.GetAnimationLength(PlayerAnimHelper.PLAYER_ATTACK);
    }

    void Start() {
        gameState = GameManager._instance.GetGameState();

        pi.SetGameState(gameState);
        if (flashPlayer)
            pi.SetPlayerType(1);
        else if (meleePlayer)
            pi.SetPlayerType(2);

        // If playing single player, turn melee character off initially
        if (gameState == 1 && meleePlayer) {
            pi.enabled = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlayer();

        if (!isDead && !isHurt && !isAttacking) {
            Movement();
            Animate();
        }
    }

    private void SwitchPlayer() {
        if (pi.GetSwitchCharPressed() && switchReady) {
            Debug.Log("Switching");
            StartCoroutine(SwitchDelay());
        }
    }

    IEnumerator SwitchDelay() {
        switchReady = false;

        otherPlayerInput.GetComponent<PlayerInput>().enabled = true;
        pi.enabled = false;

        yield return new WaitForSeconds(switchDelay);
        switchReady = true;
    }

    private void Movement() {
        if (pi.GetAttackPressed() && flashPlayer)
            return;
        else if (isAttacking && meleePlayer)
            return;

        if (!pi.GetRunInput())
            pm.Movement(pi.GetXAxis(), walkSpeed);
        else
            pm.Movement(pi.GetXAxis(), runSpeed);
    }

    private void Animate() {
        if (flashPlayer) {
            if (pi.GetAttackPressed() && pi.GetYAxis() > 0) 
                pa.AttackUpAnim();
            else if (pi.GetAttackPressed())
                pa.AttackAnim();
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

    IEnumerator MeleeAttack() {
        pa.AttackAnim();
        isAttacking = true;
        attackReady = false;
        yield return new WaitForSeconds(attackAnimLength);

        isAttacking = false;
        yield return new WaitForSeconds(attackDelay);
        attackReady = true;
    }

    public void TakeDamage(int damage) {
        if (isInvincible)
            return;

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
    }

}
