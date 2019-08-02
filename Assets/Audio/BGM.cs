using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : Singleton<BGM>
{
	protected override void Start()
	{
		base.Start();
		StartCoroutine(FadeIn());
	}

	/// <summary>
	/// Sets a new bgm and plays it if it's not the current one.
	/// </summary>
	/// <param name="newBgm">new bgm to play</param>
	public void SetBgm(AudioClip newBgm)
	{
		var audioSource = Instance.GetComponent<AudioSource>();
		audioSource.volume = 0.5f;
		if (audioSource.clip != newBgm)
		{
			audioSource.clip = newBgm;
			StartCoroutine(FadeIn());
		}
	}

	/// <summary>
	/// Stops bgm
	/// </summary>
	public void StopBgm()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeIn()
	{
		var audioSource = Instance.GetComponent<AudioSource>();
		audioSource.Play();
		var timer = 0.0f;
		while (timer < 1)
		{
			audioSource.volume = timer / 2;
			timer += Time.deltaTime;
			yield return null;
		}
		audioSource.volume = 0.5f;
	}
	
	private IEnumerator FadeOut()
	{
		var audioSource = Instance.GetComponent<AudioSource>();
		var timer = 1.0f;
		while (timer > 0)
		{
			audioSource.volume = timer / 2;
			timer -= Time.deltaTime;
			yield return null;
		}
		audioSource.volume = 0;
	}
}
