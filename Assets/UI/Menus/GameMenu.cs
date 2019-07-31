using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;

    private GameObject toggleMenuOnSelected, toggleMenuOffSelected;

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
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled)
        {
            EventSystem.current.SetSelectedGameObject(toggleMenuOnSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(toggleMenuOffSelected);
        }
    }

    public void SetToggleMenuOnSelected(GameObject obj)
    {
        toggleMenuOnSelected = obj;
    }

    public void SetToggleMenuOffSelected(GameObject obj)
    {
        toggleMenuOffSelected = obj;
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
