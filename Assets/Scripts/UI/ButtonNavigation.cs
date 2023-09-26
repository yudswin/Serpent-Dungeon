using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] buttons; // Array to hold the buttons to navigate
    private int currentIndex = 0; // Index of the currently selected button

    void Start()
    {
        // Select the first button by default
        SelectButton(currentIndex);
    }

    void Update()
    {
        // Get input from the new Input System
        var input = Keyboard.current;

        if (input != null)
        {
            // Check for Up and Down arrow key presses
            if (input.downArrowKey.wasPressedThisFrame)
            {
                // Move to the next button
                currentIndex = (currentIndex + 1) % buttons.Length;
                SelectButton(currentIndex);
            }
            else if (input.upArrowKey.wasPressedThisFrame)
            {
                // Move to the previous button
                currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
                SelectButton(currentIndex);
            }
        }
    }

    // Helper function to select a button
    void SelectButton(int index)
    {
        // Deselect all buttons
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        // Select the specified button
        buttons[index].interactable = true;

        // Set the focus on the selected button
        buttons[index].Select();
    }
}
