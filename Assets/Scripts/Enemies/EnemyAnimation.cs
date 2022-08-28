using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyAnimation : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private RuntimeAnimatorController ac;
    private string currentState;

    [Header("Anim Speed Slow")]
    [SerializeField] [Range(0, 1)]
    private float minAnimSpeed;
    private float speedToSlowAnim;
    private float animSpeed = 1f;

    [Header("Light Burn")]
    [SerializeField] private Light2D burnLight;
    [SerializeField] private float maxBurnIntensity;
    private bool isBurning;
    private float speedToIntensifyBurn;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ac = anim.runtimeAnimatorController;
    }

    void Update() {
        if (!isBurning && burnLight.intensity > 0) {
            ResetValues();
        }
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

    public void SlowAnimSpeed(float maxBurnTime) {
        speedToSlowAnim = minAnimSpeed / maxBurnTime;

        if (animSpeed > minAnimSpeed) {
            animSpeed -= Time.deltaTime * speedToSlowAnim;
            anim.SetFloat("animSpeed", animSpeed);
        } else if (animSpeed < minAnimSpeed) {
            animSpeed = minAnimSpeed;
            anim.SetFloat("animSpeed", animSpeed);
        }
    }

    public void SetBurnIntensity(float maxBurnTime) {
        if (!isBurning) {
            isBurning = true;
            speedToIntensifyBurn = maxBurnIntensity / maxBurnTime;
        }

        if (burnLight.intensity < maxBurnIntensity)
            burnLight.intensity += Time.deltaTime * speedToIntensifyBurn;
        else if (burnLight.intensity > maxBurnIntensity)
            burnLight.intensity = maxBurnIntensity;
    }

    public void ResetValues() {
        if (isBurning)
            isBurning = false;

        // Reset burn light
        if (burnLight.intensity > 0f)
            burnLight.intensity -= Time.deltaTime * speedToIntensifyBurn;
        else if (burnLight.intensity < 0f)
            burnLight.intensity = 0f;

        // Reset animation speed
        if (animSpeed < 1f) {
            animSpeed += Time.deltaTime * speedToSlowAnim;
            anim.SetFloat("animSpeed", animSpeed);
        } else if (animSpeed > 1f) {
            animSpeed = 1f;
            anim.SetFloat("animSpeed", animSpeed);
        }
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
