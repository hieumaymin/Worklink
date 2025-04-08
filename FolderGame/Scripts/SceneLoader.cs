using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName = "level2"; // 👈 Change this to your next scene name

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
