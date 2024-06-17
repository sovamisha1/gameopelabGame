using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGame;
    public Button settings;
    public Button exit;

    void Start()
    {
        startGame.onClick.AddListener(PlayGame);
        settings.onClick.AddListener(OpenSettings);
        exit.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        // Открытие меню настроек
        // Здесь можно загрузить настройки из другого Canvas
        // Либо открыть настройки в этом же Canvas
        Debug.Log("Открытие настроек");
    }

    public void QuitGame()
    {
        // Выход из игры
        Application.Quit();
    }
}
