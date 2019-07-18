using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//only used for button click sounds that would get swallowed because button loads another scene atm
[RequireComponent(typeof(AudioSource))]
public class SFX : Singleton<SFX>
{
    public void PlaySound(AudioClip ac)
    {
        Instance.GetComponent<AudioSource>().clip = ac;
        Instance.GetComponent<AudioSource>().Play();
    }
}
