using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public const int MAIN_MENU_SCENE = 0;
    public const int PLAY_SCENE = 1;

    [SerializeField] public GameObject pausePanel;
    public static void Play()
    {
        SceneManager.LoadScene(PLAY_SCENE);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public static void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
