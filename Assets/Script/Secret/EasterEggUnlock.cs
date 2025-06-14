using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggUnlock : MonoBehaviour
{
    [SerializeField] private string secretCode = "RAINBOW0";
    [SerializeField] private int bufferMaxLength = 10;
    [SerializeField] private FormManager formManager;
    [SerializeField] private Sprite rainbowBulletSprite;
    [SerializeField] private Sprite blankBulletSprite;
    
    private List<char> keyPressBuffer = new List<char>();

    private void Update()
    {
        if (Input.anyKeyDown && formManager.formList.Count == 8)
        {
            string input = Input.inputString;

            if (!string.IsNullOrEmpty(input) && input.Length == 1)
            {
                char pressedChar = char.ToUpper(input[0]);
                Debug.Log(pressedChar);
                
                keyPressBuffer.Add(pressedChar);

                if (keyPressBuffer.Count > bufferMaxLength)
                {
                    keyPressBuffer.RemoveAt(0);
                }
                
                string currentBufferString = new string(keyPressBuffer.ToArray());

                if (currentBufferString.Contains(secretCode))
                {
                    RainbowFormUnlock();
                    Debug.Log("Unlock EasterEgg");
                    keyPressBuffer.Clear();
                }
            }
        }
    }

    private void RainbowFormUnlock()
    {
        foreach (Form form in formManager.formList)
        {
            if (form.name != "BlankForm")
            {
                form.name = "RainbowForm";
                form.bulletSprite = rainbowBulletSprite;
            }
            else
            {
                form.bulletSprite = blankBulletSprite;
            }
        }
    }
}