using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] TMP_Text interactionText;
    public GameObject TextUnderlay;

    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (F)";
        TextUnderlay.SetActive(true);
    }

    public void DisableInteractionText()
    {
        if(interactionText != null)
        TextUnderlay.SetActive(false);
    }
}
