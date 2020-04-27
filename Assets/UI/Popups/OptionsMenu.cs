using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private Slider bgmSlider, sfxSlider;

    [SerializeField] 
    private Toggle fullscreenToggle, muteToggle;
    
    private Canvas canvas;

    [SerializeField]
    private bool inMainMenu = false;

    /// <summary>
    /// Objects that will be selected when toggling options/... on/off
    /// </summary>
    private GameObject onMenuToggledOnSelected, onMenuToggledOffSelected;

    public GameObject firstSelected, mainMenuSelected;

    public GameObject controls;
    
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        SetSliderValues();
        SetCheckBoxStates();
        if (!inMainMenu)
        {
            MenuButton.RemoveFocus();
        }
    }

    private void Update()
    {
        if (inMainMenu)
        {
            CheckInput();
        }
    }


    private void CheckInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (controls.activeSelf)
            {
                controls.SetActive(false);
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            else if (canvas.enabled)
            {
                canvas.enabled = false;
                EventSystem.current.SetSelectedGameObject(mainMenuSelected);
            }
        }
    }

    /// <summary>
    /// Matches slider values / game volume with PlayerPrefs saved volumes
    /// </summary>
    private void SetSliderValues()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0);
        bgmSlider.value = (float)Math.Pow(10, bgmVolume / 20);
        mixer.SetFloat("BgmVolume", bgmVolume);
        
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
        sfxSlider.value = (float)Math.Pow(10, sfxVolume / 20);
        mixer.SetFloat("SfxVolume", sfxVolume);
    }

    /// <summary>
    /// Matches checkbox states with PlayerPrefs saved values and applies checkbox effects
    /// </summary>
    private void SetCheckBoxStates()
    {
        var isFullscreen = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        var isMuted = Convert.ToBoolean(PlayerPrefs.GetInt("Mute", 0));
        muteToggle.isOn = isMuted;
        Mute(isMuted);
    }

    /// <summary>
    /// Uses Log10 of value to achieve more linear volume increase
    /// </summary>
    /// <param name="value">Value to use for volume calculation</param>
    public void SetBgmVolume(float value)
    {
        SetVolume("BgmVolume", Mathf.Log10(value) * 20);
    }

    /// <summary>
    /// Uses Log10 of value to achieve more linear volume increase
    /// </summary>
    /// <param name="value">Value to use for volume calculation</param>
    public void SetSfxVolume(float value)
    {
        SetVolume("SfxVolume", Mathf.Log10(value) * 20);
    }

    /// <summary>
    /// Sets given volume type to given volume and saves them to playerprefs
    /// </summary>
    /// <param name="volumeType">Type of volume</param>
    /// <param name="value">Value of volume</param>
    private void SetVolume(string volumeType, float value)
    {
        mixer.SetFloat(volumeType, value);
        PlayerPrefs.SetFloat(volumeType, value);
    }

    /// <summary>
    /// Toggles sound
    /// </summary>
    /// <param name="isMuted">Mutes sound if true, unmutes otherwise</param>
    public void Mute(bool isMuted)
    {
        if (isMuted)
        {
            mixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            mixer.SetFloat("MasterVolume", 0);
        }
        PlayerPrefs.SetInt("Mute", Convert.ToInt32(isMuted));
    }

    /// <summary>
    /// Toggles fullscreen
    /// </summary>
    /// <param name="isFullscreen">Sets to fullscreen if true, windowed otherwise</param>
    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(isFullscreen));
    }

    /// <summary>
    /// Toggles given menu
    /// </summary>
    /// <param name="menu"></param>
    public void ToggleMenu(GameObject menu)
    {
        var canvas = menu.GetComponent<Canvas>();
        if (!canvas)
        {
            menu.SetActive(!menu.activeSelf);
            if (menu.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(onMenuToggledOnSelected);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(onMenuToggledOffSelected);
            }
            return;
        }
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled)
        {
            EventSystem.current.SetSelectedGameObject(onMenuToggledOnSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(onMenuToggledOffSelected);
        }
    }

    /// <summary>
    /// Sets what will be selected when toggling menu on
    /// </summary>
    /// <param name="obj">object to select next</param>
    public void SetOnMenuToggledOnSelected(GameObject obj)
    {
        onMenuToggledOnSelected = obj;
    }

    /// <summary>
    /// Sets what will be selected when toggling menu off
    /// </summary>
    /// <param name="obj">object to select next</param>
    public void SetOnMenuToggledOffSelected(GameObject obj)
    {
        onMenuToggledOffSelected = obj;
    }
}
