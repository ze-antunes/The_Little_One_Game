using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // General Parameters
    // Window
    public GameObject window;

    // Indicator
    public GameObject indicator;

    //Text Component
    public TMP_Text dialogueText;

    // Dialogues list
    public List<string> dialogues;
    public List<string> newDialogues;

    //Writting speed
    public float writtingSpeed;

    // Index on dialogue
    public int index;

    // Character index
    private int charIndex;

    //Started boolean
    private bool started;

    //WaitForNext boolean
    public bool waitForNext;

    //nextLevelAvailable boolean
    public bool nextLevelAvailable;

    // Portal
    public GameObject nextLevelPortal;

    
    [Header("Sound Management")]
    public AudioSource portalSoundEffect;

    [Header("Custom Events")]
    public UnityEvent customEvent;

    private void Awake()
    {
        ToggleWindow(false);
        ToggleIndicator(false);
    }

    public void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    // Start Dialogue
    public void StartDialogue()
    {
        if (started)
        {
            return;
        }
        //Boolean to indicate that we have started
        started = true;
        //Show the window
        ToggleWindow(true);
        //Hide the indicator
        ToggleIndicator(false);
        //Start with first dialogue
        GetDialogue(0);
    }

    public void GetDialogue(int i)
    {
        //Start index at zero
        index = i;
        //Reset the char index
        charIndex = 0;
        //Clear the dialogue component text
        dialogueText.text = string.Empty;
        //Start writing
        StartCoroutine(Writing());
    }

    // End Dialogue
    public void EndDialogue()
    {
        //Disable booleans
        started = false;
        waitForNext = false;

        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);
    }

    // Writing logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];
        //Write the character
        dialogueText.text += currentDialogue[charIndex];
        //Increase the character index
        charIndex++;

        //Make sure you have reached the end of teh sentence
        if (charIndex < currentDialogue.Length)
        {
            //Wait x seconds
            yield return new WaitForSeconds(writtingSpeed);
            //Restart the same process
            StartCoroutine(Writing());
        }
        else
        {
            //End this sentence and wait for the next one
            waitForNext = true;
        }
    }

    private void Update()
    {
        if (!started)
        {
            return;
        }

        if (waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;

            if (index < dialogues.Count)
            {
                // Debug.Log($"Dialogue index: {index}");
                GetDialogue(index);
            }
            else
            {
                //End dialogue
                EndDialogue();
            }

            NextLevelAvailable();
        }
    }

    public void NextLevelAvailable()
    {
        if (index < dialogues.Count)
        {
            return;
        }
        else
        {
            nextLevelAvailable = true;
            nextLevelPortal.GetComponent<BoxCollider2D>().enabled = true;
            nextLevelPortal.SetActive(true);
            portalSoundEffect.Play();
        }
    }

    public void NextLevel()
    {
        nextLevelAvailable = true;
        nextLevelPortal.GetComponent<BoxCollider2D>().enabled = true;
        nextLevelPortal.SetActive(true);
        ResetDialogue();
        dialogues[0] = "Now you are ready to procede";
    }

    public void ResetDialogue(){
        dialogues.Clear();
        for (int i = 0; i < newDialogues.Count; i++){
            dialogues.Add(newDialogues[i]);
        }
    }
}
