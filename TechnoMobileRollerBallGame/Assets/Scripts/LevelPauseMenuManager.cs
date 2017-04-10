using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPauseMenuManager : MonoBehaviour {

    public GameObject pauseButton;
    public GameObject pauseMenuPanel;

    // Use this for initialization
    void Start () {
        pauseMenuPanel.SetActive(false);		
	}

    public void displayPauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        Time.timeScale = (pauseMenuPanel.activeSelf) ? 0 : 1;
    }

    public void restartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }


}
