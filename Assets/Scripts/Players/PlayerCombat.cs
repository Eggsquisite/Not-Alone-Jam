using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private Collider2D upColl;
    [SerializeField] private Collider2D straightColl;

    [Header("Health Values")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }
}
