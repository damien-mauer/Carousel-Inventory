using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Item> inventory = new List<Item>();
    public List<Item> notes = new List<Item>();
    public int AmountHealthItems = 0;
    public int AmountMatches = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddItem(Item ItemToAdd)
    {
       if (ItemToAdd.Type == Item.ItemType.Note)
        {
            notes.Add(ItemToAdd);
        }
        else
        {
            if (HasItem(ItemToAdd) && !ItemToAdd.UniqueItem)
            {
                foreach (Item _Item in inventory)
                {
                    if (_Item.Type == Item.ItemType.Health)
                    {
                        AmountHealthItems += ItemToAdd.Amount;
                    }
                }
            }
            if (!HasItem(ItemToAdd))
            {
                if (ItemToAdd.Type == Item.ItemType.Health) AmountHealthItems = ItemToAdd.Amount;
                inventory.Add(ItemToAdd);
            }
        }
    }

    public void RemoveItem(string ItemName)
    {
        if(inventory.Count > 0)
        {
            foreach (Item _Item in inventory)
            {
                if (_Item.ItemName == ItemName)
                {
                    inventory.Remove(_Item);
                }
            }
        }
    }

    public bool HasItem(Item ItemToHave)
    {
        foreach (Item _Item in inventory)
        {
            if (_Item.ItemName == ItemToHave.ItemName)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasNoteByID(int ID)
    {
        foreach (Item _note in notes)
        {
            if (_note.ID == ID)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasItemByID(int ID)
    {
        foreach (Item _item in inventory)
        {
            if (_item.ID == ID)
            {
                return true;
            }
        }
        return false;
    }
}
