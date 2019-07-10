using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;
    
    public void EndBgm()
    {
        BGM.Instance.SetBgm(bgm);
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
