using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class LetterTileData
{
    public GameObject tileObject;
    public char letter;
    public Vector2Int position;
}

[System.Serializable]
public class WordData
{
    public string word;
    public string hint;
    public int startX;
    public int startY;
    public bool horizontal;
}

public class GridManager : MonoBehaviour
{
    public GameObject letterTilePrefab;
    public Transform gridContainer;
    public TMP_InputField answerInput;
    public TextMeshProUGUI resultText;
    public Transform hintPanel;
    public GameObject hintTextPrefab;
    public string levelFileName = "level1"; // Without .txt extension

    public GameObject nextLevelButton; // 🔁 Button shown after all words are guessed

    public int gridSizeX = 9;
    public int gridSizeY = 9;

    public RetryManager retryManager;

    private Dictionary<string, List<LetterTileData>> wordTileMap = new Dictionary<string, List<LetterTileData>>();
    private Dictionary<Vector2Int, GameObject> tileMap = new Dictionary<Vector2Int, GameObject>();

    private char[,] grid;
    private List<WordData> wordDataList = new List<WordData>();
    private List<string> validWords = new List<string>();
    private List<string> wordHints = new List<string>();

    private HashSet<string> guessedWords = new HashSet<string>(); // 🔁 Tracks correct words

    void Start()
    {
        grid = new char[gridSizeX, gridSizeY];
        LoadWordData();
        GenerateHints();
        GenerateGrid();

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false); // 🔁 Hide next level button initially
        }
    }

    void LoadWordData()
    {
        TextAsset file = Resources.Load<TextAsset>(levelFileName);
        if (file == null)
        {
            Debug.LogError($"Level file {levelFileName} not found in Resources!");
            return;
        }

        string[] lines = file.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            string[] parts = line.Trim().Split('|');
            if (parts.Length != 5) continue;

            WordData data = new WordData
            {
                word = parts[0].Trim().ToUpper(),
                startX = int.Parse(parts[1]),
                startY = int.Parse(parts[2]),
                horizontal = parts[3].Trim().ToUpper() == "H",
                hint = parts[4].Trim()
            };

            wordDataList.Add(data);
            validWords.Add(data.word);
            wordHints.Add(data.hint);
        }
    }

    public void GenerateHints()
    {
        for (int i = 0; i < wordHints.Count; i++)
        {
            GameObject hintObj = Instantiate(hintTextPrefab, hintPanel);
            var hintText = hintObj.GetComponent<TextMeshProUGUI>();
            hintText.text = $"{i + 1}. {wordHints[i]}";
        }
    }

    public void GenerateGrid()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in wordDataList)
        {
            PlaceWord(data.word, data.startX, data.startY, data.horizontal);
        }

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject tile = Instantiate(letterTilePrefab, gridContainer);
                tile.name = $"Tile_{x}_{y}";
                tile.transform.localScale = Vector3.one;

                TMP_Text text = tile.GetComponentInChildren<TMP_Text>();
                Image bg = tile.GetComponent<Image>();

                tileMap[pos] = tile;

                if (grid[x, y] == '\0')
                {
                    text.text = "";
                    text.color = new Color(0, 0, 0, 0);
                    if (bg != null) bg.enabled = false;
                }
                else
                {
                    text.text = "";
                    text.color = new Color(0, 0, 0, 0);
                    if (bg != null) bg.enabled = true;
                }
            }
        }
    }

    void PlaceWord(string word, int startX, int startY, bool horizontal)
    {
        List<LetterTileData> tileList = new List<LetterTileData>();

        for (int i = 0; i < word.Length; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;

            if (x >= gridSizeX || y >= gridSizeY)
            {
                Debug.LogWarning($"'{word}' out of bounds at ({x},{y})");
                return;
            }

            grid[x, y] = word[i];

            Vector2Int pos = new Vector2Int(x, y);
            tileList.Add(new LetterTileData
            {
                position = pos,
                letter = word[i],
                tileObject = null
            });
        }

        wordTileMap[word] = tileList;
    }

    public void CheckWord()
    {
        string typed = answerInput.text.ToUpper().Trim();
        bool isCorrect = false;

        if (validWords.Contains(typed) && !guessedWords.Contains(typed))
        {
            resultText.text = "Correct!";
            resultText.color = Color.green;
            isCorrect = true;
            guessedWords.Add(typed);
            RevealCorrectWord(typed);

            if (guessedWords.Count >= validWords.Count && nextLevelButton != null)
            {
                nextLevelButton.SetActive(true);
            }
        }
        else
        {
            resultText.text = "Incorrect!";
            resultText.color = Color.red;
        }

        retryManager.CheckTries(isCorrect);
        answerInput.text = "";
    }

    private void RevealCorrectWord(string word)
    {
        if (wordTileMap.ContainsKey(word))
        {
            List<LetterTileData> tiles = wordTileMap[word];

            foreach (var tileData in tiles)
            {
                Vector2Int pos = tileData.position;
                GameObject tile = tileMap[pos];

                TMP_Text text = tile.GetComponentInChildren<TMP_Text>();
                Image bg = tile.GetComponent<Image>();

                text.text = tileData.letter.ToString();
                text.color = Color.black;
                if (bg != null) bg.enabled = true;
            }
        }
    }

    public void ResetGrid()
    {
        guessedWords.Clear();
        answerInput.text = "";
        resultText.text = "";

        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform hint in hintPanel)
        {
            Destroy(hint.gameObject);
        }

        wordTileMap.Clear();
        tileMap.Clear();
        wordDataList.Clear();
        validWords.Clear();
        wordHints.Clear();

        grid = new char[gridSizeX, gridSizeY];

        LoadWordData();
        GenerateHints();
        GenerateGrid();

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }
    }
}
