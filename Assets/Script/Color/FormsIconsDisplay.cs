using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsIconsDisplay : MonoBehaviour
{
    public FormManager formManager; 

    [Header("UI Slots")]
    public Image[] formIconSlots; 

    void Start()
    {
        if (formManager == null)
        {
            enabled = false;
            return;
        }

        if (formIconSlots == null || formIconSlots.Length == 0)
        {
            enabled = false;
            return;
        }

        UpdateDisplayedForms();
    }

    public void UpdateDisplayedForms()
    {
        for (int i = 0; i < formIconSlots.Length; i++)
        {
            if (formIconSlots[i] != null)
            {
                formIconSlots[i].sprite = null;
                formIconSlots[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
                formIconSlots[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < formManager.formList.Count && i < formIconSlots.Length; i++)
        {
            Form currentForm = formManager.formList[i];
            Image currentSlot = formIconSlots[i];

            if (currentForm == null)
            {
                Debug.LogWarning($"Form at index {i} in collectedForms is null. Skipping display for this slot.");
                continue;
            }
            if (currentSlot == null)
            {
                Debug.LogWarning($"UI Image slot at index {i} is null. Cannot display form icon.");
                continue;
            }

            if (currentForm.formIcon != null)
            {
                currentSlot.sprite = currentForm.formIcon;
                currentSlot.color = Color.white;
            }
            else
            {
                currentSlot.sprite = null;
            }
        }

        for (int i = formManager.formList.Count; i < formIconSlots.Length; i++)
        {
            if (formIconSlots[i] != null)
            {
                formIconSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
