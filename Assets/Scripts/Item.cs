using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    //Interaction Type
    public enum InteractionType
    {
        NONE,
        PickUp,
        Examine
    }

    //Item Type
    public enum ItemType
    {
        Static,
        Consumable
    }

    [Header("Attributes")]
    public InteractionType interactionType;
    public ItemType itemType;

    [Header("Examine")]
    public string descriptionText;
    public Sprite image;

    [Header("Custom Events")] 
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    //Collider Trigger
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 8;
    }

    public void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.PickUp:
                //Add the object to the PickUpItems list
                FindObjectOfType<InventorySystem>()
                    .PickUp(gameObject); 
                break;
            case InteractionType.Examine:
                //Call the Examine item in the interaction system
                FindObjectOfType<InteractionSystem>()
                    .ExamineItem(this);
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        //Call all custom events
        customEvent.Invoke();
    }
}
