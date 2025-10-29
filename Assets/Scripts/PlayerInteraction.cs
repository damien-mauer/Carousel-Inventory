using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerInteraction : MonoBehaviour
{
    //How far items can be away before interaction
    public float playerReach = 3f;
    //Stores which object player is currently looking at
    Interactable currentInteractable;
    private GameObject player;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject crosshair;
    [HideInInspector] public bool inventorySwitch = false;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule");
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventorySwitch = !inventorySwitch;
            if (inventorySwitch)
            {
                inventoryUI.SetActive(true);
                InventoryUI.instance.OpenInventory();
            }
            else
            {
                InventoryUI.instance.ExitInventory();
            }

        }
    }

    public void Open()
    {
        InventoryUI.instance.ExitInventory();
    }

    //Raycasting
    void CheckInteraction()
    {
        RaycastHit hit;
        //shots ray straight from camera center
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //if collides with anything within player reach
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            //if looking at an interactable obj
            if (hit.collider.tag == "Interactable")
            {
                //get reference to obj
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();
                //if there is a currentInteractable and it is not the newInteractable
                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }
                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else //if new interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else //if not an interactable
            {
                DisableCurrentInteractable();
            }
        }
        else // if nothing in reach
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        if (currentInteractable != null)
        {
            HUDController.instance.EnableInteractionText(currentInteractable.message);
        }
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
