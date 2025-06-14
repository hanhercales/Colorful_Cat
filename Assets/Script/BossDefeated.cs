using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDefeated : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject boss;
    [SerializeField] private float cd;

    private void Update()
    {
        if (boss == null)
        {
            victoryPanel.SetActive(true);
                    
            cd -= Time.deltaTime;
        }
        if(cd <= 0) SceneManager.LoadScene("MainMenu");
    }
}
