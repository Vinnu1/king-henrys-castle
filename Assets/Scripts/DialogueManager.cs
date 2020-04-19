using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogText;
    public Animator animator;
    public Animator loadLevelAnimator;
    private Queue<string> sentences;
    private AudioSource Audio;
    public AudioClip textClip;
    public AudioClip letterClip;
    public Button nextButton;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        Audio = GetComponent<AudioSource>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        PlayAudio("letter");
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
       yield return new WaitForSeconds(0.3f);
       nextButton.Select();
       DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        animator.SetTrigger("Next");
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //dialogText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        nextButton.Select();
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            PlayAudio("text");
            dialogText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        PlayAudio("letter");
        animator.SetBool("IsOpen", false);
        EventSystem.current.SetSelectedGameObject(null);
        if(SceneManager.GetActiveScene().name == "FinalRoom")
        {
            loadLevelAnimator.SetTrigger("FadeIn");
        }
    }

    void PlayAudio(string type)
    {
        if(type == "text")
        {
            Audio.PlayOneShot(textClip, 0.06f);
        }
        else if(type == "letter")
        {
            Audio.Stop();
            Audio.PlayOneShot(letterClip, 1f);
        }
    }
}