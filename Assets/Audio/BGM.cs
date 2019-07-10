using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : Singleton<BGM>
{
	public void SetBgm(AudioClip newBgm)
	{
		var audioSource = GetComponent<AudioSource>();
		if (!audioSource.clip == newBgm)
		{
			audioSource.clip = newBgm;
			audioSource.Play();
		}
	}
}
