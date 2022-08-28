using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip burningSFX;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;
    // Start is called before the first frame update
    void Start()
    {
        // I know not to do this but I'm on a time crunch :(
        source = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
    }

    public void PlayBurnSFX() {
        source.PlayOneShot(burningSFX);
    }
    public void PlayAttackSFX() {
        source.PlayOneShot(attackSFX);
    }
    public void PlayHurtSFX() {
        source.PlayOneShot(hurtSFX);
    }
    public void PlayDeathSFX() {
        source.PlayOneShot(deathSFX);
    }
}
