using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public void GoToNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

    // Try to parse number from scene name like "level1"
        if (currentScene.StartsWith("level"))
        {
            string numberPart = currentScene.Substring(5); // After "level"
            if (int.TryParse(numberPart, out int levelNumber))
            {
                string nextScene = "level" + (levelNumber + 1);
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
