using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : MonoBehaviour
{
	public void LoadNextPanel(string name)
	{
		Fade.FadeToBlack(name);
	}

	/// <summary>
	/// Loads given level using loading screen
	/// </summary>
	/// <param name="name">Level to load</param>
	public void LoadLevel(string name)
	{
		LoadingScreen.NextLevelName = name;
		Fade.FadeToBlack("Loading Screen");
	}

	private void Update()
	{
		CheckInput();
	}

	private void CheckInput()
	{
		if (Input.anyKeyDown)
		{
			GetComponent<Button>().onClick.Invoke();
		}
	}
}
