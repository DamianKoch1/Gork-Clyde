using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : Singleton<BGM>
{
	/// <summary>
	/// Sets a new bgm and plays it if it's not the current one.
	/// </summary>
	/// <param name="newBgm">new bgm to play</param>
	public void SetBgm(AudioClip newBgm)
	{
		var audioSource = GetComponent<AudioSource>();
		if (!audioSource.clip == newBgm)
		{
			audioSource.clip = newBgm;
			audioSource.Play();
		}
	}

	/// <summary>
	/// Stops bgm
	/// </summary>
	public void StopBgm()
	{
		GetComponent<AudioSource>().Stop();
	}
}
