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

    public void AddForm(Form form)
    {
        formList.Add(form);
    }

    public void ChangeForm(int formIndex)
    {
        if(Input.GetKeyDown(formList[formIndex].activationKey))
            targetAnimator.runtimeAnimatorController = formList[formIndex].animatorController;
    }
}
