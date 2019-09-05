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
            OnBackPressed();
        }

    }

    /// <summary>
    /// Hides the highest layered popup (controls > options > pause), if none are active pause the game
    /// </summary>
    private void OnBackPressed()
    {
        var optionsCanvas = optionsMenu.GetComponent<Canvas>();
        var pauseCanvas = pauseMenu.GetComponent<Canvas>();
        if (!pauseCanvas.enabled)
        {
            Pause(pauseCanvas);   
        }
        else if (optionsMenu.controls.activeSelf)
        {
            HideControls();
        }
        else if (optionsCanvas.enabled)
        {
            HideOptions(optionsCanvas);
        }
        else
        {
            Unpause(pauseCanvas);
        }
    }

    /// <summary>
    /// Hides pause menu, sets timescale to 1, disables cursor, clears button selection
    /// </summary>
    /// <param name="pauseCanvas">Canvas of pause menu</param>
    private void Unpause(Canvas pauseCanvas)
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
        MenuButton.FocusNothing();
        Cursor.visible = false;
    }

    /// <summary>
    /// Shows pause menu, sets timescale to 0, enables cursor, selects assigned pause menu button
    /// </summary>
    /// <param name="pauseCanvas">Canvas of pause menu</param>
    private void Pause(Canvas pauseCanvas)
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
    private void HideOptions(Canvas optionsCanvas)
    { 
        optionsCanvas.enabled = false;
        EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
    }

    
}
