using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    private GameObject selectOnMenuToggledOn, selectOnMenuToggledOff;

    [SerializeField] 
    private AudioClip bgm;

    [SerializeField] 
    private bool setNewBgm = false;

    private void Start()
    {
        Cursor.visible = true;
        if (setNewBgm)
        {
            BGM.Instance.SetBgm(bgm);
        }
    }


    /// <summary>
    /// Stops bgm
    /// </summary>
    public void EndBgm()
    {
        BGM.Instance.StopBgm();
    }


    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        Fade.FadeToBlack(sceneName);
    }

    public void LoadLoadingScreen(string sceneName)
    {
        LoadingScreen.NextLevelName = sceneName;
        Fade.FadeToBlack("Loading Screen");
    }
    
    /// <summary>
    /// Toggles pause/options menu
    /// </summary>
    /// <param name="menu">menu to toggle</param>
    public void ToggleMenu(GameObject menu)
    {
        var canvas = menu.GetComponent<Canvas>();
        if (!canvas)
        {
            menu.SetActive(!menu.activeSelf);
            if (menu.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(selectOnMenuToggledOn);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(selectOnMenuToggledOff);
            }
            return;
        }
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled)
        {
            EventSystem.current.SetSelectedGameObject(selectOnMenuToggledOn);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(selectOnMenuToggledOff);
        }
    }

    public void SetOnMenuToggledOnSelected(GameObject obj)
    {
        selectOnMenuToggledOn = obj;
    }

    public void SetOnMenuToggledOffSelected(GameObject obj)
    {
        selectOnMenuToggledOff = obj;
    }
    
    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        Fade.FadeToBlack(SceneManager.GetActiveScene().name);
    }

}
