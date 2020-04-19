using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            animator.SetTrigger("FadeIn");
        }
    }
}
