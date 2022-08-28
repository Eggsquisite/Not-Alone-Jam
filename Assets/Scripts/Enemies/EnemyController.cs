using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType{
        Ghoul,
        Summoner,
        Spitter
    }

    public enum EnemyState{
        Spawning,
        Idle,
        Walking,
        Attacking,
        Hurt,
        Death
    }

    [Header("Components")]
    private SpitterAI spitAI;
    private SummonerAI summAI;
    private EnemyAnimation ea;
    private EnemyCombat ec;
    private EnemyMovement em;
    private SpriteRenderer sp;

    [Header("Enemy Specific")]
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public EnemyState enemyState;

    [Header("Attack Values")]
    private bool attackReady = true;
    private float attackAnimLength;
    private float timeToDeath;


    void Awake()
    {
        if (enemyType == EnemyType.Ghoul) 
            if (em == null) em = GetComponent<EnemyMovement>();
        else if (enemyType == EnemyType.Summoner)
            if (summAI == null) summAI = GetComponent<SummonerAI>();
        else if (enemyType == EnemyType.Spitter)
            if (spitAI == null) spitAI = GetComponent<SpitterAI>();

        if (ea == null) ea = GetComponent<EnemyAnimation>();
        if (ec == null) ec = GetComponent<EnemyCombat>();
        if (em == null) em = GetComponent<EnemyMovement>();
        if (sp == null) sp = GetComponent<SpriteRenderer>();

        enemyState = EnemyState.Idle;
        attackAnimLength = ea.GetAnimationLength(EnemyAnimHelper.ENEMY_ATTACK);
        timeToDeath = ea.GetAnimationLength(EnemyAnimHelper.ENEMY_DEATH);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyType == EnemyType.Ghoul) {
            Ghoul();
        }
    }

    private void Ghoul() {
        Death();
        Burning();
        Hit();
        MoveAndAnimate();
        Attack();
    }

    private void Death() {
        if (ec.GetFullyBurned() && enemyState != EnemyState.Death) {
            enemyState = EnemyState.Death;
            StartCoroutine(DeathDestroy());
        }
    }

    IEnumerator DeathDestroy() {
        ea.DeathAnim();
        yield return new WaitForSeconds(timeToDeath);
        Destroy(gameObject);
    }

    private void Burning() {
        if (enemyState == EnemyState.Death)
            return;
        if (ec.GetBurnedState()) {
            //enemyState = EnemyState.Hurt;

            ea.SlowAnimSpeed(ec.GetMaxBurnTime());
            ea.SetBurnIntensity(ec.GetMaxBurnTime());
            em.SlowMoveSpeed(ec.GetMaxBurnTime());
        }
        else {
            ea.ResetValues();
            em.ResetMoveSpeed();
        }
    }

    private void Hit() {

    }

    private void MoveAndAnimate() {
        if (enemyState == EnemyState.Idle || enemyState == EnemyState.Walking) {
            if (attackReady) {
                var tmp = em.Movement();
                if (tmp != 0) {
                    enemyState = EnemyState.Walking;
                    ea.WalkAnim();
                    
                    // Face player direction
                    if (tmp == 1)
                        transform.localScale = new Vector2(1f, transform.localScale.y);
                    else if (tmp == -1)
                        transform.localScale = new Vector2(-1f, transform.localScale.y);
                }
                else if (tmp == 0) {
                    enemyState = EnemyState.Idle;
                    ea.IdleAnim();   
                }
            } else {
                ea.IdleAnim();   
                enemyState = EnemyState.Idle;
            }
        }
    }

    private void Attack() {
        if (em.Attack() != 0 && attackReady && (enemyState == EnemyState.Walking || enemyState == EnemyState.Idle)) {
            if (em.Attack() == 1) 
                transform.localScale = new Vector2(-1f, transform.localScale.y);
            else if (em.Attack() == 2) 
                transform.localScale = new Vector2(1f, transform.localScale.y);
            
            ea.AttackAnim();
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay() {
        attackReady = false;
        enemyState = EnemyState.Attacking;
        yield return new WaitForSeconds(attackAnimLength);

        enemyState = EnemyState.Idle;
        yield return new WaitForSeconds(em.GetAttackDelay());
        attackReady = true;
    }
}
