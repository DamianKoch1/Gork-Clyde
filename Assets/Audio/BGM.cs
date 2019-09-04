using System.Collections;
using UnityEngine;

/// <summary>
/// Plays BGM, stays loaded between scenes
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BGM : Singleton<BGM>
{
	
	/// <summary>
	/// Sets a new bgm and plays it if it's not the current one
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
	/// Fades bgm out
	/// </summary>
	public void StopBgm()
	{
		StartCoroutine(FadeOut());
	}

	/// <summary>
	/// Fades instance's AudioSource volume in
	/// </summary>
	/// <returns></returns>
	private IEnumerator FadeIn()
	{
		StopCoroutine(FadeOut());
		var audioSource = Instance.GetComponent<AudioSource>();
		audioSource.Play();
		var timer = 0.0f;
		while (timer < 0.5f)
		{
			audioSource.volume = timer;
			timer += Time.deltaTime;
			yield return null;
		}
		audioSource.volume = 0.5f;
	}
	
	/// <summary>
	/// Fades instance's AudioSource volume out
	/// </summary>
	/// <returns></returns>
	private IEnumerator FadeOut()
	{
		StopCoroutine(FadeIn());
		var audioSource = Instance.GetComponent<AudioSource>();
		var timer = 0.5f;
		while (timer > 0)
		{
			audioSource.volume = timer;
			timer -= Time.deltaTime;
			yield return null;
		}
		audioSource.volume = 0;
	}
}
