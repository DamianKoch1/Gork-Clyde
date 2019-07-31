using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;
    
    [SerializeField]
    private GameObject pauseMenu, optionsMenu;
    

    private void Start()
    {
        BGM.Instance.SetBgm(bgm);
    }

    private void Update()
    {
        CheckInput();
    }

    
    private void CheckInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            var optionsCanvas = optionsMenu.GetComponent<Canvas>();
            var pauseCanvas = pauseMenu.GetComponent<Canvas>();
            if (!pauseCanvas.enabled)
            {
                pauseCanvas.enabled = true;
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(pauseMenu.GetComponentInChildren<Button>().gameObject);
            }
            else if (optionsCanvas.enabled)
            {
                optionsCanvas.enabled = false;
                EventSystem.current.SetSelectedGameObject(optionsMenu.GetComponentInChildren<Button>().gameObject);
            }
            else
            {
                pauseCanvas.enabled = false;
                Time.timeScale = 1;
                MenuButton.FocusNothing();
            }
        }

        if (Input.GetButtonDown("DebugRestart"))
        {
            Restart();
        }
    }

    public void Restart()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
