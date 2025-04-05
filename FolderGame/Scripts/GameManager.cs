using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Text hintText;
    public Button hintButton;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public AudioSource hintSound, winSound, gameOverSound;

    private List<string> words = new List<string> { "NEAR", "AREA", "RANE" };
    private HashSet<string> foundWords = new HashSet<string>();
    private int hintCount = 3;

    void Start()
    {
        hintButton.onClick.AddListener(UseHint);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void UseHint()
    {
        if (hintCount > 0)
        {
            string wordToReveal = GetRandomWord();
            if (!string.IsNullOrEmpty(wordToReveal))
            {
                hintText.text = "Gợi ý: " + wordToReveal;
                hintCount--;

                if (hintSound) hintSound.Play();

                if (hintCount == 0)
                {
                    hintButton.interactable = false;
                }
            }
        }
    }

    string GetRandomWord()
    {
        List<string> remainingWords = new List<string>();
        foreach (string word in words)
        {
            if (!foundWords.Contains(word))
            {
                remainingWords.Add(word);
            }
        }
        if (remainingWords.Count > 0)
        {
            return remainingWords[Random.Range(0, remainingWords.Count)];
        }
        return null;
    }

    public void CheckWinCondition()
    {
        if (foundWords.Count == words.Count)
        {
            winPanel.SetActive(true);
            winPanel.GetComponent<Animator>().SetTrigger("Show");
            if (winSound) winSound.Play();
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<Animator>().SetTrigger("Show");
        if (gameOverSound) gameOverSound.Play();
    }
}
