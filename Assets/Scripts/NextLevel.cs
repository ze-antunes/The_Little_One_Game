using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    // Prop
    public GameObject prop;

    void Update()
    {
        if (GetComponent<BoxCollider2D>().enabled && prop)
        {
            HideProp();
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (
                FindObjectOfType<InventorySystem>().items.Contains(
                    FindObjectOfType<InventorySystem>().bowNarrow
                )
            )
            {
                FindObjectOfType<LevelLoader>().LoadNextLevelWithItem(4);
            }
            else if (
                FindObjectOfType<InventorySystem>().items.Contains(
                    FindObjectOfType<InventorySystem>().mirror
                )
            )
            {
                FindObjectOfType<LevelLoader>().LoadNextLevelWithItem(6);
            }
            else if (
                FindObjectOfType<InventorySystem>().items.Contains(
                    FindObjectOfType<InventorySystem>().coin
                )
            )
            {
                FindObjectOfType<LevelLoader>().LoadNextLevelWithItem(7);
            }
            else
            {
                FindObjectOfType<LevelLoader>().LoadNextLevel();
            }
        }
    }

    public void HideProp()
    {
        prop.SetActive(false);
    }

}
