using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    [SerializeField] FormManager formManager;
    [SerializeField] GameObject portal;

    private void Update()
    {
        if(formManager.formList.Count == 8)
            portal.SetActive(true);
    }
}
