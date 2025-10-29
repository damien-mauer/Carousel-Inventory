using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public int ID = -1;
    public string ItemName;
    public Sprite Sprite;
    [field: TextArea]
    public string Description;
    [field: TextArea]
    public string LongDescription;
    public bool UniqueItem = true;
    public int Amount = 1;
    public enum ItemType { Health, Key, PuzzleItem, Note };
    public ItemType Type;
    [Header("Health")]
    public int HealthRestorationAmount;
    [Header("Key")]
    public DoorType OpenableDoorType;
    public enum DoorType { StoneDoor, MetalDoor };
    [Header("Puzzle Item")]
    public bool IsCombinable = false;
    public int CombinableWithID = -1;
    public int CombinesToID = -1;
    public int SolvesID = -1;
    public Item referenceToPuzzle;
    public GameObject gameObject;
    [Header("Note")]
    public bool AddToInventory;
    public bool isImage;
    [field: TextArea]
    public string Message;
    public int Number = -1;
}