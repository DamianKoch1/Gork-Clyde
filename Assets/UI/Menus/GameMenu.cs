using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    public void EndBgm()
    {
        GameObject bgm = GameObject.FindGameObjectWithTag("bgm");
        Destroy(bgm);
    }


    public void LoadCharacterSelection(string nextSceneName)
    {
        Time.timeScale = 1;
        CharacterSelection.NEXT_LEVEL_NAME = nextSceneName;
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
