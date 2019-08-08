using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Shows once at start to recommend using controllers
/// </summary>
public class ControllerPopup : MonoBehaviour
{
	
	[SerializeField] 
	private GameObject selectOnHide;

	private void Start()
	{
		ShowOnNewGame();
	}

	/// <summary>
	/// Shows this popup if never shown this save
	/// </summary>
	private void ShowOnNewGame()
	{
		if (PlayerPrefs.HasKey("ControllerRecommendationShown")) return;
		Show(true);
		PlayerPrefs.SetInt("ControllerRecommendationShown", 1);
	}

	/// <summary>
	/// Toggles popup
	/// </summary>
	/// <param name="show"></param>
	public void Show(bool show)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}

		if (show)
		{
			EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(selectOnHide);
		}
	}
}
