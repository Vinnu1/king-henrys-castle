using UnityEngine;

public class EnemyAnimationActions : MonoBehaviour
{
    public AudioClip attackClip, hitClip;
    private AudioSource Audio;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    void AttackSound()
    {
        Audio.PlayOneShot(attackClip, 0.5f);
    }

    void HitSound()
    {
        Audio.PlayOneShot(hitClip, 0.5f);
    }

}
