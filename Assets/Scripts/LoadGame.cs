using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public Animator animator;
    public AudioSource Audio;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if(SceneManager.GetActiveScene().name == "TitleScreen")
            {
                Audio.Play();
                animator.SetTrigger("FadeIn");
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
