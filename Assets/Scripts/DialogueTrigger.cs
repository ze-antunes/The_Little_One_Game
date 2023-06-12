using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    public bool playerDetected;

    [Header("Sound Management")]

    public AudioSource npcInterractionSoundEffect;

    //Detect  trigger with player
    private void OnTriggerEnter2D(Collider2D collision){
        //If we triggered the player enable playerdetected and show indicator
        if(collision.tag == "Player"){
            playerDetected = true;
            dialogueScript.ToggleIndicator(playerDetected);
            dialogueScript.EndDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        //If we lost trigger with the player disable playerdetected and hide indicator
        if(collision.tag == "Player"){
            playerDetected = false;
            dialogueScript.ToggleIndicator(playerDetected);
        }
    }

    //While detected if we interract start the dialogue
    private void Update(){
        if(playerDetected && Input.GetKeyDown(KeyCode.E)){
            dialogueScript.StartDialogue();
            npcInterractionSoundEffect.Play();
        }
    } 
}
