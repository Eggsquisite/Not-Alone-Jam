using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float xAxis, yAxis;
    private bool runHeld, attackPressed, aimUpHeld;

    // Update is called once per frame
    void Update()
    {
        // MOVEMENT INPUTS
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        // RUN INPUT
        if (Input.GetKeyDown(KeyCode.LeftShift))
            runHeld = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            runHeld = false;

        // ATTACK INPUTS
        if (Input.GetKeyDown(KeyCode.F))
            attackPressed = true;
        else if (Input.GetKeyUp(KeyCode.F))
            attackPressed = false;
        
    }

    public float GetXAxis() {
        return xAxis;
    }
    public float GetYAxis() {
        return yAxis;
    }
    public bool GetRunInput() {
        return runHeld;
    }
    public bool GetAttackPressed() {
        return attackPressed;
    }
}
