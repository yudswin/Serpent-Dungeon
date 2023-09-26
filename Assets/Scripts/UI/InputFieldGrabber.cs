using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldGrabber : MonoBehaviour
{
    [Header("The value from the input field")]
    [SerializeField] private string inputText;

    [Header("Reaction to the player")]
    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        DisplayReactionToInput();
    }

    private void DisplayReactionToInput()
    {
        //show reaction
    }
}
