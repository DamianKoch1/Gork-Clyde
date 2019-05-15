using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{

    [SerializeField] 
    private AudioMixer mixer;
    
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
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
}
