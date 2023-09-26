using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldGrabber : MonoBehaviour
{
    [Header("The value from the input field")]
    [SerializeField] private string inputText;

    public void GrabFromInputField(string input)
    {
        if (input == null)
        {
            inputText = "Name";
        } 
        else
        {
            inputText = input;
        } 

        GameManager.playerName = inputText;
    }

}
