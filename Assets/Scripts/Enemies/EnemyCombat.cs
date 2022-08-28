using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Components")]
    private Collider2D coll;

    [Header("Burn Values")]
    [SerializeField] private float burnMaxTime;
    private float burnTime;
    private bool isBurned, fullyBurned;

    

    // Start is called before the first frame update
    void Awake()
    {
        if (coll == null) coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBurned)
            StopBurning();
        else
            IsBurning();
    }

    // Flashlight hit
    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "AttackLight" && !isBurned) {
            isBurned = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "AttackLight") {
            isBurned = false;
        }
    }

    public void IsBurning() {
        if (burnTime < burnMaxTime)
            burnTime += Time.deltaTime;
        else if (burnTime >= burnMaxTime && !fullyBurned) {
            fullyBurned = true;
            burnTime = burnMaxTime;
        }
    }

    public void StopBurning() {
        if (burnTime == 0)
            return;
        else if (burnTime > 0) 
            burnTime -= Time.deltaTime;
        else if (burnTime < 0)
            burnTime = 0f;
    }

    public bool GetBurnedState() {
        return isBurned;
    }

    public bool GetFullyBurned() {
        return fullyBurned;
    }

    public float GetMaxBurnTime() {
        return burnMaxTime;
    }

    // Melee hit
}
