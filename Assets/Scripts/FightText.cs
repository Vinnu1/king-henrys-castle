using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightText : MonoBehaviour
{
    public Animator animator;
    public AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("Remove");
        Audio.Stop();
    }
}
