using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormManager : MonoBehaviour
{
    [System.Serializable]
    public class Form
    {
        public RuntimeAnimatorController animatorController;
        public KeyCode activationKey;
    }
    
    public List<Form> formList = new List<Form>();
    public Animator targetAnimator;

    [SerializeField] private GameObject[] formDisplaySlot;

    public void AddForm(Form form)
    {
        formList.Add(form);
        FormDisplayUpdate();
    }

    public void ChangeForm(int formIndex)
    {
        if(Input.GetKeyDown(formList[formIndex].activationKey))
            targetAnimator.runtimeAnimatorController = formList[formIndex].animatorController;
    }

    private void FormDisplayUpdate()
    {
        for (int i = 0; i < formList.Count; i++)
        {
            formDisplaySlot[i].SetActive(true);
        }
    }

    private void OnEnable()
    {
        FormDisplayUpdate();
    }
}
