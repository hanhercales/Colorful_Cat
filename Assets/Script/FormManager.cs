using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormManager : MonoBehaviour
{
    public List<RuntimeAnimatorController> formList = new List<RuntimeAnimatorController>();
    public Animator targetAnimator;

    public void AddForm(RuntimeAnimatorController form)
    {
        formList.Add(form);
    }

    public void ChangeForm(RuntimeAnimatorController form)
    {
        targetAnimator.runtimeAnimatorController = form;
    }
}
