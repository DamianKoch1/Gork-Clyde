﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuButton : MonoBehaviour
{
    public void EndBgm()
    {
        GameObject bgm = GameObject.FindGameObjectWithTag("bgm");
        Destroy(bgm);
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
