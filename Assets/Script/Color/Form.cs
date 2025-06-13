using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewForm", menuName = "Form")]
public class Form : ScriptableObject
{
    public new string name;
    public Sprite formIcon;
    public RuntimeAnimatorController animatorController;
    public KeyCode activationKey;
    public Sprite bulletSprite;
}