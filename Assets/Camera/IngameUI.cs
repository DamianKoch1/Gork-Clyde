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
    private GameMenu pauseMenu;

    [SerializeField] 
    private OptionsMenu optionsMenu;

    [SerializeField] 
    private GameObject pauseFirstSelected;
    

    private void Start()
    {
        BGM.Instance.SetBgm(bgm);
        Cursor.visible = false;
        MenuButton.FocusNothing();

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
                Cursor.visible = true;
                pauseCanvas.enabled = true;
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
            }
            else if (optionsMenu.controls.activeSelf)
            {
                optionsMenu.controls.SetActive(false);
                EventSystem.current.SetSelectedGameObject(optionsMenu.firstSelected);
            }
            else if (optionsCanvas.enabled)
            {
                optionsCanvas.enabled = false;
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
            }
            else
            {
                pauseCanvas.enabled = false;
                Time.timeScale = 1;
                MenuButton.FocusNothing();
                Cursor.visible = false;
            }
        }

        if (Input.GetButtonDown("DebugRestart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    
}
