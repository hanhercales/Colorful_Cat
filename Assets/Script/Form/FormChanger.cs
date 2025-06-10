using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormChanger : MonoBehaviour
{
    public FormManager.Form form;
    public FormManager formManager;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            formManager.AddForm(form);
            Destroy(this.gameObject);
        }
    }
}