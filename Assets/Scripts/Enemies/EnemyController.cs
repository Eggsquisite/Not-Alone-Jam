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
    private GhoulAI ghoulAI;
    private SpitterAI spitAI;
    private SummonerAI summAI;
    private EnemyAnimation ea;
    private SpriteRenderer sp;

    [Header("Enemy Specific")]
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public EnemyState enemyState;

    [Header("Attack Values")]
    private bool attackReady = true;
    private float attackAnimLength;


    void Awake()
    {
        if (enemyType == EnemyType.Ghoul)
            if (ghoulAI == null) ghoulAI = GetComponent<GhoulAI>();
        else if (enemyType == EnemyType.Summoner)
            if (summAI == null) summAI = GetComponent<SummonerAI>();
        else if (enemyType == EnemyType.Spitter)
            if (spitAI == null) spitAI = GetComponent<SpitterAI>();

        if (ea == null) ea = GetComponent<EnemyAnimation>();
        if (sp == null) sp = GetComponent<SpriteRenderer>();

        enemyState = EnemyState.Idle;
        attackAnimLength = ea.GetAnimationLength(EnemyAnimHelper.ENEMY_ATTACK);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyType == EnemyType.Ghoul) {
            Ghoul();
        }
    }

    private void Ghoul() {
        if (enemyState == EnemyState.Idle || enemyState == EnemyState.Walking) {
            if (attackReady) {
                var tmp = ghoulAI.Movement();
                if (tmp != 0) {
                    ea.WalkAnim();
                    enemyState = EnemyState.Walking;
                    
                    // Face player direction
                    if (tmp == 1)
                        transform.localScale = new Vector2(1f, transform.localScale.y);
                    else if (tmp == -1)
                        transform.localScale = new Vector2(-1f, transform.localScale.y);
                }
                else if (tmp == 0) {
                    ea.IdleAnim();   
                    enemyState = EnemyState.Idle;
                }
            } else {
                ea.IdleAnim();   
                enemyState = EnemyState.Idle;
            }
        }

        // Attack right
        if (ghoulAI.Attack() != 0 && attackReady && (enemyState == EnemyState.Walking || enemyState == EnemyState.Idle)) {
            if (ghoulAI.Attack() == 1) 
                transform.localScale = new Vector2(-1f, transform.localScale.y);
            else if (ghoulAI.Attack() == 2) 
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
        yield return new WaitForSeconds(ghoulAI.GetAttackDelay());
        attackReady = true;
    }
}
