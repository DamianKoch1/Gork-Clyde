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

    private void Start()
    {
        Cursor.visible = true;
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
        SceneManager.LoadScene(sceneName);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
