using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject skillTree;

    private void Start()
    {
        pauseMenu.SetActive(false);
        skillTree.SetActive(false);
        Debug.Log(pauseMenu);
        Debug.Log(GameIsPaused);

        Resume();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.pause = true;
        
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        skillTree.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false;

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameIsPaused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        GameIsPaused = false;
    }

    public void HubWorld()
    {
        SceneManager.LoadScene(1);
        GameIsPaused = false;
    }

    public void SkillTree()
    {
        pauseMenu.SetActive(false);
        skillTree.SetActive(true);
    }
}
