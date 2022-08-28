using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private int gameState;
    private float xAxis, yAxis;
    private bool runHeld, attackPressed, aimUpHeld, switchCharPressed;
    private bool meleePlayer, flashPlayer;

    void OnDisable() {
        ResetVariables();
    }

    void OnEnable() {
        ResetVariables();
    }

    private void ResetVariables() {
        xAxis = 0f;
        yAxis = 0f;
        runHeld = false;
        attackPressed = false;
        aimUpHeld = false;
        switchCharPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 1)
            SinglePlayer();
        else if (gameState == 2)
            LocalCoop();
    }

    private void LocalCoop() {
        if (flashPlayer) {
            // MOVEMENT INPUTS
            xAxis = Input.GetAxisRaw("HorizontalP1");
            yAxis = Input.GetAxisRaw("VerticalP1");

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
        else if (meleePlayer) {
            // MOVEMENT INPUTS
            xAxis = Input.GetAxisRaw("HorizontalP2");
            yAxis = Input.GetAxisRaw("VerticalP2");

            // RUN INPUT
            if (Input.GetKeyDown(KeyCode.RightShift))
                runHeld = true;
            else if (Input.GetKeyUp(KeyCode.RightShift))
                runHeld = false;

            // ATTACK INPUTS
            if (Input.GetKeyDown(KeyCode.RightControl))
                attackPressed = true;
            else if (Input.GetKeyUp(KeyCode.RightControl))
                attackPressed = false;
        }
    }

    private void SinglePlayer() {
        // MOVEMENT INPUTS
        xAxis = Input.GetAxisRaw("HorizontalP1");
        yAxis = Input.GetAxisRaw("VerticalP1");

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

        // SWITCH CHARACTER
        if (Input.GetKeyDown(KeyCode.Tab)) {
            switchCharPressed = true;
        }
    }

    public void SetPlayerType(int type) {
        if (type == 1)
            flashPlayer = true;
        else if (type == 2)
            meleePlayer = true;
    }

    public void SetGameState(int state) {
        gameState = state;
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

    public bool GetSwitchCharPressed() {
        return switchCharPressed;
    }
}
