using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Components")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackAISearchRange;

    [Header("Health Values")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Animation attack for melee player
    public void AttackEnemies() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage them
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<EnemyController>().IsHurt(attackDamage);
        }
    }

    public int AIAttack () {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, attackAISearchRange, enemyLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, attackAISearchRange, enemyLayer);
        Debug.DrawRay(transform.position, Vector3.left * attackAISearchRange, Color.green);
        Debug.DrawRay(transform.position, Vector3.right * attackAISearchRange, Color.red);

        if (hitLeft.collider != null && hitRight.collider != null) {
            var tmpLeft = Mathf.Abs(hitLeft.transform.position.x - transform.position.x);
            var tmpRight = Mathf.Abs(hitRight.transform.position.x - transform.position.x);

            if (tmpLeft < tmpRight)
                return 1;
            else if (tmpRight < tmpLeft)
                return 2;
        }
        else if (hitLeft.collider != null)
            return 1;
        else if (hitRight.collider != null)
            return 2;
        else if (hitRight.collider == null && hitLeft.collider == null)
            return 0;

        return 0;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }
}
