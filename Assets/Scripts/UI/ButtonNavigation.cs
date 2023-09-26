using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonNavigation : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // Array to hold the buttons to navigate
    [SerializeField] private TMP_Text descriptionText;
    private int _currentIndex = 0; // Index of the currently selected button
    

    void Start()
    {
        // Select the first button by default
        SelectButton(_currentIndex);
    }

    void Update()
    {
        // Get input from the new Input System
        var input = Keyboard.current;

        if (input != null)
        {
            // Check for Up and Down arrow key presses
            if (input.rightArrowKey.wasPressedThisFrame)
            {
                // Move to the next button
                _currentIndex = (_currentIndex + 1) % buttons.Length;
                SelectButton(_currentIndex);
            }
            else if (input.leftArrowKey.wasPressedThisFrame)
            {
                // Move to the previous button
                _currentIndex = (_currentIndex - 1 + buttons.Length) % buttons.Length;
                SelectButton(_currentIndex);
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
        descriptionText.text = string.Format('"'+"{0}"+'"',buttons[index].name);
    }

    public void BonusSelected(string bonus)
    {
        BonusType type = BonusType.SmallSize;
        switch (bonus)
        {
            case "SmallSize":
                type = BonusType.SmallSize;
                break;
            case "WallSlide":
                type = BonusType.WallSlide;
                break;
            case "AddTime":
                type = BonusType.AddTime;
                break;
        }

        GameManager.starterBonus = type;
        SceneManager.LoadScene("InGame");
    }
}
