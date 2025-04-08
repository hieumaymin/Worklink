using UnityEngine;
using UnityEngine.SceneManagement;

public interface IMainMenu
{
    void OpenAbout();
    void OpenHowToPlay();
    void PlayGame();
    void QuitGame();
}

public class MainMenu : MonoBehaviour, IMainMenu
{
    // Chuyển đến màn chơi
    public void PlayGame()
    {
        SceneManager.LoadScene("level1"); // Đảm bảo "GameScene" đã có trong Build Settings.
    }

    // Chuyển đến màn hướng dẫn
    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("HowToPlayScene"); // Đảm bảo "HowToPlayScene" đã có trong Build Settings.
    }

    // Chuyển đến màn thông tin
    public void OpenAbout()
    {
        SceneManager.LoadScene("AboutScene"); // Đảm bảo "AboutScene" đã có trong Build Settings.
    }

    // Thoát game
    public void QuitGame()
    {
        Application.Quit(); // Thoát game
        Debug.Log("Game Closed");
    }
    
}