
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message; //text displayed when hovering object
    GameObject inventoryUI;
    public Item ScriptableItem;

    public UnityEvent onInteraction;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
    }

    //Will call everything set in the inspector
    public virtual void Interact()
    {
        onInteraction.Invoke();

        if (ScriptableItem.Type == Item.ItemType.Note)
        {
            GameObject.FindGameObjectWithTag("NoteUI").GetComponent<NoteUI>().OpenNote(ScriptableItem);
            if (ScriptableItem.AddToInventory)
            {
                InventoryManager.instance.AddItem(ScriptableItem);
            }
        }
        else
        {
            InventoryManager.instance.AddItem(ScriptableItem);
        }
    }

    public void DisableOutline() 
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        if(outline != null)
        outline.enabled = true;
    }
}
