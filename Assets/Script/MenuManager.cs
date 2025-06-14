using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string collectedForms = "CollectedForms";
    [SerializeField] private Button continueButton;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
        
        if(!PlayerPrefs.HasKey(collectedForms))
            continueButton.interactable = false;
    }

    public void NewGame()
    {
        if (PlayerPrefs.HasKey(collectedForms))
        {
            PlayerPrefs.DeleteKey(collectedForms);
            PlayerPrefs.Save();
        }
    }

    public void SetAudioVolumn(float volumn)
    {
        audioSource.volume = volumn;
    }
}
