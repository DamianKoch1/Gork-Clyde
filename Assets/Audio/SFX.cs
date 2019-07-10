using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//only used for button click sounds that would get swallowed because button loads another scene atm
[RequireComponent(typeof(AudioSource))]
public class SFX : Singleton<SFX>
{
    public void PlaySound(AudioClip ac)
    {
        GetComponent<AudioSource>().clip = ac;
        GetComponent<AudioSource>().Play();
    }
}
