using UnityEngine;

public class AnimationActions : MonoBehaviour
{
    public Player player;
    public AudioClip attackClip, jumpClip, walkClip, hitClip, deathClip, stopClip;
    private AudioSource Audio;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    void AttackStaminaReduce()
    {
            player.stamina = player.stamina - 0.3f;
    }

    void AttackSound()
    {
        Audio.PlayOneShot(attackClip, 0.5f);
    }

    void JumpSound()
    {
        Audio.PlayOneShot(jumpClip, 0.3f);
    }

    void WalkSound()
    {
        Audio.PlayOneShot(walkClip, 1f);
    }

    void HitSound()
    {
        Audio.PlayOneShot(hitClip);
    }

    void DeathSound()
    {
        Audio.PlayOneShot(deathClip, 0.5f);
    }

    public void StopSound()
    {
        Audio.PlayOneShot(stopClip, 0.5f);
    }

}
