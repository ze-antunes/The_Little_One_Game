using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;

    // Detection Radius
    public const float detectionRadius = .2f;

    // Detection Layer
    public LayerMask detectionLayer;

    //Cached Trigger Object
    public GameObject detectedIObject;

    [Header("Examination Parameters")]
    //Examine window Object
    public GameObject examineWindow;
    public Image examineImage;
    public TextMeshProUGUI examineText;
    public bool isExamining;

    [Header("Others")]
    //List of picked items
    public List<GameObject> pickedItems = new List<GameObject>();

    void Update()
    {
        if (DetectedObject())
        {
            if (InteractInput())
            {
                detectedIObject.GetComponent<Item>().Interact();
            }
        }
    }

    public bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool DetectedObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(
            detectionPoint.position,
            detectionRadius,
            detectionLayer
        );
        if (obj == null)
        {
            detectedIObject = null;
            return false;
        }
        else
        {
            detectedIObject = obj.gameObject;
            return true;
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    // }

    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            Debug.Log("CLOSE");
            //Hide the Examine Window
            examineWindow.SetActive(false);

            isExamining = false;
        }
        else
        {
            Debug.Log("Examine");
            //Show the item's image in the middle
            if (item.image)
                examineImage.sprite =item.image;
            else
                examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            //Write description text underneath the image
            examineText.text = item.descriptionText;
            //Display the Examine Window
            examineWindow.SetActive(true);

            isExamining = true;
        }
    }
}
