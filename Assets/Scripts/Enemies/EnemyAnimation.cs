using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private RuntimeAnimatorController ac;

    private string currentState;

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
    public float GetAnimationLength(string newAnim) {
        return AnimHelper.GetAnimClipLength(ac, newAnim);
    }

    // ALL ENEMY ANIMATIONS
    public void SpawnAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_SPAWN); }
    public void AlertAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_ALERT); }
    public void IdleAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_IDLE); }
    public void WalkAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_WALK); }
    public void AttackAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_ATTACK); }
    public void HurtAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_HURT); }
    public void DeathAnim() { PlayAnimation(EnemyAnimHelper.ENEMY_DEATH); }
}
