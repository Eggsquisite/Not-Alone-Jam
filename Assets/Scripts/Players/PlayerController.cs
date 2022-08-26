using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerInput pi;
    private PlayerMovement pm;
    private PlayerAnimation pa;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    void Awake() 
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pi = GetComponent<PlayerInput>();
        pm = GetComponent<PlayerMovement>();
        pa = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Animate();
    }

    private void Movement() {
        if (pi.GetAttackPressed())
            return;

        if (!pi.GetRunInput())
            pm.Movement(pi.GetXAxis(), walkSpeed);
        else
            pm.Movement(pi.GetXAxis(), runSpeed);
    }

    private void Animate() {
        if (pi.GetAttackPressed() && pi.GetYAxis() > 0)
            pa.AttackUpAnim();
        else if (pi.GetAttackPressed())
            pa.AttackAnim();
        else if (pi.GetXAxis() == 0)
            pa.IdleAnim();
        else if (pi.GetXAxis() != 0 && !pi.GetRunInput())
            pa.WalkAnim();
        else if (pi.GetXAxis() != 0 && pi.GetRunInput())
            pa.RunAnim();
    }
}
