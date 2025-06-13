using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string collectedForms = "CollectedForms";
    
    public void NewGame()
    {
        if (PlayerPrefs.HasKey(collectedForms))
        {
            PlayerPrefs.DeleteKey(collectedForms);
            PlayerPrefs.Save();
        }
    }
}
