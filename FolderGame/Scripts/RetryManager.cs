using UnityEngine;
using TMPro;  // Make sure to add this line
using UnityEngine.UI;

public class RetryManager : MonoBehaviour
{
    public Button tryAgainButton;  // Reference to the "Try again?" button
    public GridManager gridManager; // Reference to the GridManager script
    public int maxTries = 3; // Max number of tries allowed
    private int currentTries = 0; // The current number of tries

    public TextMeshProUGUI remainingTriesText; // Reference to the TextMeshPro text that shows remaining tries

    void Start()
    {
        // Ensure the "Try again?" button is hidden at the start
        tryAgainButton.gameObject.SetActive(false);

        // Update the Remaining Tries Text at the start
        UpdateRemainingTriesText();

        // Add listener for the Try again button
        tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
    }

    public void CheckTries(bool isCorrect)
    {
        if (!isCorrect)
        {
            currentTries++;
            UpdateRemainingTriesText(); // Update tries text

            if (currentTries >= maxTries)
            {
                ShowTryAgainButton();
            }
        }
    }

    void UpdateRemainingTriesText()
    {
        int remainingTries = maxTries - currentTries;
        remainingTriesText.text = $"Tries left: {remainingTries}"; // Update the UI Text
    }

    void ShowTryAgainButton()
    {
        tryAgainButton.gameObject.SetActive(true); // Show the retry button
    }

    void OnTryAgainButtonClicked()
    {
        currentTries = 0; // Reset the tries counter
        tryAgainButton.gameObject.SetActive(false); // Hide the retry button

        gridManager.ResetGrid(); // Call GenerateGrid method from GridManager to reset the puzzle
        UpdateRemainingTriesText(); // Reset the remaining tries text after a retry
    }
    public void RetryGame()
    {
        gridManager.ResetGrid();
    }
}
