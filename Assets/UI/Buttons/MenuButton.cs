using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickSound;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().clip = clickSound;
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().Play();
    }
    
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
