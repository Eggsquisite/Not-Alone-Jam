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

    public void TakeDamage(int damage) {
        currentHealth -= damage;
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }
}
