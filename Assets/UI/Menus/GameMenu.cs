using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
