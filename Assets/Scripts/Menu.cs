using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Button ResumeButton;
    public Button ExitButton;
    public AudioSource Audio;
    public Animator animator;

    void Start()
    {
        ResumeButton.onClick.AddListener(ResumeButtonPress);
        ExitButton.onClick.AddListener(ExitButtonPress);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !animator.GetBool("IsOpen"))
        {
            if (!MenuCanvas.enabled)
            {
                //kind of does the work
                Time.timeScale = 0;
                FindObjectOfType<Player>().TogglePause();
                Audio.Play();
                MenuCanvas.enabled = true;
                ResumeButton.Select();
            }
        }
    }

    private void ResumeButtonPress()
    {
        if (MenuCanvas.enabled)
        {
            Audio.Play();
            EventSystem.current.SetSelectedGameObject(null);
            MenuCanvas.enabled = false;
            Time.timeScale = 1;
            FindObjectOfType<Player>().TogglePause();
        }
    }

    private void ExitButtonPress()
    {
        Application.Quit();
    }


}
