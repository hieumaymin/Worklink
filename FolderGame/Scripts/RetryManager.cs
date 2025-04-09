using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RetryManager : MonoBehaviour
{
    public Button tryAgainButton;
    public GridManager gridManager;
    public int maxTries = 3;
    private int currentTries = 0;

    public TextMeshProUGUI remainingTriesText;

    void Start()
    {
        tryAgainButton.gameObject.SetActive(false);
        UpdateRemainingTriesText();
        tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
    }

    public void CheckTries(bool isCorrect)
    {
        if (currentTries >= maxTries) return;

        if (!isCorrect)
        {
            currentTries++;
            UpdateRemainingTriesText();

            if (currentTries >= maxTries)
            {
                ShowTryAgainButton();
                gridManager.DisableInput();
            }
        }
    }

    void UpdateRemainingTriesText()
    {
        int remainingTries = Mathf.Max(0, maxTries - currentTries);
        remainingTriesText.text = $"Tries left: {remainingTries}";
    }

    void ShowTryAgainButton()
    {
        tryAgainButton.gameObject.SetActive(true);
    }

    void OnTryAgainButtonClicked()
    {
        currentTries = 0;
        tryAgainButton.gameObject.SetActive(false);
        gridManager.ResetGrid();
        UpdateRemainingTriesText();
    }

    public int GetTriesLeft()
    {
        return Mathf.Max(0, maxTries - currentTries);
    }

    public void RetryGame()
    {
        gridManager.ResetGrid();
    }
}