using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains most functionality menu buttons may have
/// </summary>
public class GameMenu : MonoBehaviour
{

    /// <summary>
    /// Objects that will be selected when toggling options/... on/off
    /// </summary>
    private GameObject selectOnMenuToggledOn, selectOnMenuToggledOff;

    [SerializeField] 
    private bool setNewBgm = false;
    
    [SerializeField] 
    private AudioClip bgm;

    private void Start()
    {
        Cursor.visible = true;
        SetBgm();
    }

    /// <summary>
    /// If setNewBgm is true, starts playing new bgm if not already playing
    /// </summary>
    private void SetBgm()
    {
        if (setNewBgm)
        {
            BGM.Instance.SetBgm(bgm);
        }
    }


    /// <summary>
    /// Fades bgm out
    /// </summary>
    public void EndBgm()
    {
        BGM.Instance.StopBgm();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
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

    /// <summary>
    /// Sets what will be selected when toggling menu on
    /// </summary>
    /// <param name="obj">object to select next</param>
    public void SetOnMenuToggledOnSelected(GameObject obj)
    {
        selectOnMenuToggledOn = obj;
    }

    /// <summary>
    /// Sets what will be selected when toggling menu off
    /// </summary>
    /// <param name="obj">object to select next</param>
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
    
    /// <summary>
    /// Restarts current level
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        Fade.FadeToBlack(SceneManager.GetActiveScene().name);
    }

}
