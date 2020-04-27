using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;
    
    [SerializeField]
    private GameMenu pauseMenu;

    [SerializeField] 
    private OptionsMenu optionsMenu;

    [SerializeField, Tooltip("Object initially selected when pause menu is opened")] 
    private GameObject pauseFirstSelected;

    private Canvas optionsCanvas;
    private Canvas pauseCanvas;
    

    private void Start()
    {
        BGM.Instance.SetBgm(bgm);
        Cursor.visible = false;
        MenuButton.RemoveFocus();

        optionsCanvas = optionsMenu.GetComponent<Canvas>();
        pauseCanvas = pauseMenu.GetComponent<Canvas>();
    }

    private void Update()
    {
        CheckInput();
    }

    
    private void CheckInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OnBackPressed();
        }

    }

    /// <summary>
    /// Hides the highest layered popup (controls > options > pause), if none are active pause the game
    /// </summary>
    private void OnBackPressed()
    {
        
        if (!pauseCanvas.enabled)
        {
            Pause();   
        }
        else if (optionsMenu.controls.activeSelf)
        {
            HideControls();
        }
        else if (optionsCanvas.enabled)
        {
            HideOptions();
        }
        else
        {
            Unpause();
        }
    }

    /// <summary>
    /// Hides pause menu, sets timescale to 1, disables cursor, clears button selection
    /// </summary>
    private void Unpause()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
        MenuButton.RemoveFocus();
        Cursor.visible = false;
    }

    /// <summary>
    /// Shows pause menu, sets timescale to 0, enables cursor, selects assigned pause menu button
    /// </summary>
    private void Pause()
    {
        Cursor.visible = true;
        pauseCanvas.enabled = true;
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
    }

    /// <summary>
    /// Hides controls popup
    /// </summary>
    private void HideControls()
    {
        optionsMenu.controls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(optionsMenu.firstSelected);
    }

    /// <summary>
    /// Hides Options menu
    /// </summary>
    /// <param name="optionsCanvas">Canvas of options menu</param>
    private void HideOptions()
    { 
        optionsCanvas.enabled = false;
        EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
    }

    
}
