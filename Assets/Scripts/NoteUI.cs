using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    public Image noteUI;
    public GameObject player;
    [SerializeField] GameObject crosshair;
    public TMP_Text noteUIContent;
    public Image close;
    Item ScriptableItem;
    public Sprite standardNoteImage;

    public void OpenNote(Item note)
    {
        ScriptableItem = note;
        noteUIContent.text = note.Message;
        noteUI.enabled = true;
        close.enabled = true;
        player.GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        crosshair.SetActive(false);
        if (note.isImage)
        {
            noteUIContent.enabled = false;
            noteUI.sprite = note.Sprite;
        }
        else
        {
            noteUIContent.enabled = true;
            noteUI.sprite = standardNoteImage;
        }
    }

    public void CloseNote()
    {
        noteUI.enabled = false;
        player.GetComponent<FirstPersonController>().enabled = true;
        noteUIContent.enabled = false;
        close.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);
    }
}
