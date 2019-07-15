using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;
    
    /// <summary>
    /// Stops bgm
    /// </summary>
    public void EndBgm()
    {
        BGM.Instance.StopBgm();
    }


    public void LoadCharacterSelection(string nextSceneName)
    {
        Time.timeScale = 1;
        CharacterSelection.StoryPanelName = nextSceneName;
        SceneManager.LoadScene("Character Selection");
    }


    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Toggles pause/options menu
    /// </summary>
    /// <param name="menu">menu to toggle</param>
    public void ToggleMenu(GameObject menu)
    {
        var canvas = menu.GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
