using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    [Header("General Parameters")]
    //List of items picked up
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> medals = new List<GameObject>();

    //Flag indicates if the inventory is open or not
    public bool isOpen;

    [Header("Narative Items")]
    public GameObject bowNarrow;
    public GameObject coin;
    public GameObject mirror;

    [Header("UI Items Section")]
    //Inventory System Window
    public GameObject ui_Window;
    public Image[] items_images;
    public Image[] medals_images;

    [Header("UI Item Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (items.Contains(bowNarrow))
        {
            FindObjectOfType<Shooting>().CanShoot();
            FindObjectOfType<PlayerMovement>().animator.SetInteger("AnimState", 1);
        }

        if (items.Contains(mirror))
        {
            FindObjectOfType<PlayerMovement>().animator.SetInteger("AnimState", 2);
        }

        if (items.Contains(coin))
        {
            FindObjectOfType<PlayerMovement>().animator.SetInteger("AnimState", 3);
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        ui_Window.SetActive(isOpen);
        // Update_UI();
        Update_Medals();
    }

    //Add the item to the items list
    public void PickUp(GameObject item)
    {
        if (items.Count > 0)
        {
            items.Add(item);
            StateController.items.Add(item);
            //Disable the object
            item.SetActive(false);

            float x = FindObjectOfType<PlayerMovement>().attackPoint.transform.position.x;
            float y = FindObjectOfType<PlayerMovement>().attackPoint.transform.position.y;
            float z = FindObjectOfType<PlayerMovement>().attackPoint.transform.position.z;

            items[0].transform.position = new Vector3(x, y, z);
            items[0].SetActive(true);
            items.Remove(items[0]);
            StateController.items.Remove(items[0]);
        }
        else
        {
            items.Add(item);
            StateController.items.Add(item);
            //Disable the object
            item.SetActive(false);
        }
        Update_UI();
        ShowDescription(0);
    }

    //Add the item to the items list
    public void Medal(GameObject medal)
    {
        medals.Add(medal);
        StateController.medals.Add(medal);
        Debug.Log(medal);
        Update_Medals();
    }

    // Refresh the UI elementes in the inventory window
    void Update_UI()
    {
        HideAll();
        // For each item in the "items" list
        // Show it in the respective slot in the "items_images"
        for (int i = 0; i < items.Count; i++)
        {
            items_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            // items_images[i].gameObject.SetActive(true);
        }
    }

    // Refresh the UI medals in the inventory window
    void Update_Medals()
    {
        // For each item in the "medals" list
        // Show it in the respective slot in the "medals_images"
        for (int i = 0; i < medals.Count; i++)
        {
            medals_images[i].sprite = medals[i].GetComponent<SpriteRenderer>().sprite;
            medals_images[i].gameObject.SetActive(true);
        }
    }

    // Hide all the items ui images
    void HideAll()
    {
        foreach (var i in items_images)
        {
            i.gameObject.SetActive(false);
        }
        HideDescription();
    }

    public void ShowDescription(int id)
    {
        //Set the Image
        description_Image.sprite = items_images[id].sprite;
        //Set the Title
        description_Title.text = items[id].name;
        //Set the Text
        description_Text.text = items[id].GetComponent<Item>().descriptionText;
        //Show the elements
        description_Image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        description_Image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }

    public void Consume(int id)
    {
        if (items[id].GetComponent<Item>().itemType == Item.ItemType.Consumable)
        {
            Debug.Log($"CONSUMED {items[id].name}");
            // Call the consume custom event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            // Destroy the item
            Destroy(items[id], 0.1f);
            // Clear the item from the list
            items.Remove(items[id]);
            Update_UI();
        }
    }
}
