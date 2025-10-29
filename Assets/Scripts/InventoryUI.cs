using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Canvas inventoryUI;
    private GameObject player;

    [Header("General UI")]
    [SerializeField] GameObject crosshair;
    public Image[] images = new Image[5];
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemAmount;
    public Image noteImage;
    public TMP_Text noteContent;
    public TMP_Text noteNumber;
    public GameObject ScrollView;
    private Item itemToCombine = null;
    public Button use;
    public TMP_Text useButtonText;
    public Button combine;
    public TMP_Text combineButtonText;
    public Button examine;

    [Header("Navigation Buttons")]
    public Button navigateItemLeft;
    public Button navigateItemRight;
    public Button navigateNoteLeft;
    public Button navigateNoteRight;
    [Header("Combine and Examine Mode")]
    public Canvas combineMode;
    public Canvas examinationMode;
    public Image examinedItem;
    public TextMeshProUGUI examinationText;
    bool isExamineMode = false;
    bool isCombineMode = false;

    public static InventoryUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (inventoryUI.enabled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        player = GameObject.Find("PlayerCapsule");
    }


    private void Update()
    {
        if (inventoryUI.enabled)
        {
            // Wenn Items im Inventory sind, lade diese
            if (InventoryManager.instance.inventory.Count > 0)
            {
                UpdateInventory();
            }
            // Wenn mehr als ein Item im Inventory ist, erlaube Navigation
            if (InventoryManager.instance.inventory.Count > 1)
            {
                navigateItemLeft.interactable = true;
                navigateItemRight.interactable = true;
            }
            // Wenn kein Item im Inventory ist, zeige leeren Item-Inventory
            if (InventoryManager.instance.inventory.Count <= 0)
            {
                for (int i = 0; i < images.Length - 1; i++)
                {
                    images[i].enabled = false;
                    images[i].sprite = null;
                    itemName.text = "";
                    itemDescription.text = "There are no items in the inventory.";
                    use.interactable = false;
                    combine.interactable = false;
                    examine.interactable = false;
                    itemAmount.enabled = false;
                }
                navigateItemLeft.interactable = false;
                navigateItemRight.interactable = false;
            }
            // Wenn Notizen im Inventory sind, lade diese
            if (InventoryManager.instance.notes.Count > 0)
            {
                // Wenn mehr als eine Notiz im Inventory ist, erlaube Navigation
                if (InventoryManager.instance.notes.Count > 1)
                {
                    navigateNoteLeft.interactable = true;
                    navigateNoteRight.interactable = true;
                }
                // Wenn die Notiz aus Text besteht, zeige den Text anstelle eines Bildes an
                if (!InventoryManager.instance.notes[0].isImage)
                {
                    noteContent.text = InventoryManager.instance.notes[0].Message;
                    noteContent.enabled = true;
                    noteImage.sprite = null;
                    ScrollView.SetActive(true);
                }
                // Ansonsten zeige das Bild an
                else
                {
                    noteContent.enabled = false;
                    noteImage.sprite = InventoryManager.instance.notes[0].Sprite;
                    ScrollView.SetActive(false);
                }
                noteNumber.text = "#" + InventoryManager.instance.notes[0].Number;
            }
            // Wenn keine Notizen im Invenory sind, zeige leeren Notiz-Inventory
            else
            {
                noteImage.sprite = null;
                noteContent.text = "There are no notes in the inventory.";
                noteNumber.text = "";
                navigateNoteLeft.interactable = false;
                navigateNoteRight.interactable = false;
            }
        }

        // Im Combined Mode können Items zusammengefügt werden
        if (isCombineMode)
        {
            examine.interactable = false;
            if (InventoryManager.instance.inventory[0].Type == Item.ItemType.PuzzleItem)
            {
                use.interactable = true;
            }
            examine.interactable = false;
            combineMode.enabled = true;
            useButtonText.text = "Choose";
            combineButtonText.text = "Cancel";
        }
        if (!isCombineMode)
        {
            combineMode.enabled = false;
            useButtonText.text = "Use";
            combineButtonText.text = "Combine";
            itemToCombine = null;
        }
        // Im Examine Mode können Items genauer betrachtet werden
        if (isExamineMode)
        {
            examinationMode.enabled = true;
            examinationText.text = InventoryManager.instance.inventory[0].LongDescription;
            examinedItem.sprite = InventoryManager.instance.inventory[0].Sprite;
        }
        if (!isExamineMode)
        {
            examinationMode.enabled = false;
        }
    }

    // Lade die derzeit enthaltenen Items und Notizen
    void UpdateInventory()
    {
        // Zeige Sprites für alle existierenden Items an
        for (int i = 0; i < images.Length; i++)
        {
            if (InventoryManager.instance.inventory.ElementAtOrDefault(i) != null)
            {
                images[i].enabled = true;
                images[i].sprite = InventoryManager.instance.inventory[i].Sprite;
            }
            else
            {
                images[i].enabled = false;
                images[i].sprite = null;
            }
        }

        itemName.text = InventoryManager.instance.inventory[0].ItemName;
        itemDescription.text = InventoryManager.instance.inventory[0].Description;

        if (InventoryManager.instance.inventory[0].Type == Item.ItemType.Health)
        {
            // use.interactable = true;
            itemAmount.text = "(" + InventoryManager.instance.AmountHealthItems.ToString() + ")";
            itemAmount.enabled = true;
        }
        else
        {
            use.interactable = false;
            itemAmount.enabled = false;
        }

        combine.interactable = InventoryManager.instance.inventory[0].IsCombinable;
        examine.interactable = true;
        navigateItemLeft.interactable = true;
        navigateItemRight.interactable = true;
    }

    public void NavigateItemsRight()
    {
        int index = 0;
        Item item = InventoryManager.instance.inventory[index];
        InventoryManager.instance.inventory.RemoveAt(index);
        InventoryManager.instance.inventory.Insert(InventoryManager.instance.inventory.Count, item);
    }

    public void NavigateItemsLeft()
    {
        int index = InventoryManager.instance.inventory.Count - 1;
        Item item = InventoryManager.instance.inventory[index];
        InventoryManager.instance.inventory.RemoveAt(index);
        InventoryManager.instance.inventory.Insert(0, item);
    }

    public void NavigateNotesRight()
    {
        int index = 0;
        Item item = InventoryManager.instance.notes[index];
        InventoryManager.instance.notes.RemoveAt(index);
        InventoryManager.instance.notes.Insert(InventoryManager.instance.notes.Count, item);
    }

    public void NavigateNotesLeft()
    {
        int index = InventoryManager.instance.notes.Count - 1;
        Item item = InventoryManager.instance.notes[index];
        InventoryManager.instance.notes.RemoveAt(index);
        InventoryManager.instance.notes.Insert(0, item);
    }

    public void CombineItemButton()
    {
        isCombineMode = !isCombineMode;
        itemToCombine = InventoryManager.instance.inventory[0];
    }

    public void ExamineItemButton()
    {
        isExamineMode = true;
    }

    public void ExamineItemCloseButton()
    {
        isExamineMode = false;
    }

    // Bestimmt was passiert wenn ein Item benutz wird (hier nur um Items zusammenzufügen)
    public void UseItemButton()
    {
        if (isCombineMode)
        {
            if (InventoryManager.instance.inventory[0].ID == itemToCombine.CombinableWithID)
            {
                InventoryManager.instance.AddItem(itemToCombine.referenceToPuzzle);
                InventoryManager.instance.inventory.Remove(itemToCombine);
                InventoryManager.instance.inventory.Remove(InventoryManager.instance.inventory[0]);
                isCombineMode = false;
            }
        }
    }

    public void OpenInventory()
    {
        inventoryUI.gameObject.SetActive(true);
        PauseGame();
        player.GetComponent<PlayerInteraction>().inventorySwitch = true;
    }

    public void PauseGame()
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        crosshair.SetActive(false);
    }

    public void ExitInventory()
    {
        inventoryUI.gameObject.SetActive(false);
        UnpauseGame();
        player.GetComponent<PlayerInteraction>().inventorySwitch = false;
        isCombineMode = false;
        isExamineMode = false;
    }

    public void UnpauseGame()
    {
        player.GetComponent<FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);
    }
}
