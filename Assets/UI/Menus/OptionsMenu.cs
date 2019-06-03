using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{

    [SerializeField] 
    private AudioMixer mixer;

    [SerializeField] 
    private Slider BgmSlider, SfxSlider;

    private Canvas canvas;

    [SerializeField] 
    private bool inMainMenu = false;
    
    private void Start()
    {
        SetSliderValues();
        canvas = GetComponent<Canvas>();
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
            if (canvas.enabled)
            {
                canvas.enabled = false;
            }
        }
    }

    private void SetSliderValues()
    {
        float temp;
        
        mixer.GetFloat("BgmVolume", out temp);
        BgmSlider.value = (float)Math.Pow(10, temp / 20);
        
        mixer.GetFloat("SfxVolume", out temp);
        SfxSlider.value = (float)Math.Pow(10, temp / 20);
    }
    
    public void SetBgmVolume(float value)
    {
        mixer.SetFloat("BgmVolume", Mathf.Log10(value) * 20);
    }
   
    public void SetSfxVolume(float value)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }

    public void Mute(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            mixer.SetFloat("MasterVolume", 0);

        }
    }

    public void Fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
    
    public void ToggleMenu(GameObject menu)
    {
        var canvas = menu.GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }
}
