using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private RuntimeAnimatorController ac;

    [Header("Animation States")]
    private string animState, currentState;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        ac = anim.runtimeAnimatorController;
    }

    // Animation Helper Functions ////////////////////////////////////////
    private void PlayAnimation(string newAnim) {
        AnimHelper.ChangeAnimationState(anim, ref currentState, newAnim);
    }
    private void ReplayAnimation(string newAnim) {
        AnimHelper.ReplayAnimation(anim, ref currentState, newAnim);
    }
    private float GetAnimationLength(string newAnim) {
        return AnimHelper.GetAnimClipLength(ac, newAnim);
    }

    // PLAYER ANIMATION METHODS
    public void IdleAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_IDLE);
    }
    public void WalkAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_WALK);
    }
    public void RunAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_RUN);
    }
    public void AttackAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_ATTACK);
    }
    public void AttackUpAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_ATTACK_UP);
    }
    public void HurtAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_HURT);
    }
    public void DeathAnim() {
        PlayAnimation(PlayerAnimHelper.PLAYER_DEATH);
    }
}
