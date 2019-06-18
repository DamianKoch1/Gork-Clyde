using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    public static string NEXT_LEVEL_NAME;

    [SerializeField]
    private Image loadingBar;

    private void Start()
    {
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(NEXT_LEVEL_NAME);
        loadingBar.fillAmount = 0;
        while (!loading.isDone)
        {
            loadingBar.fillAmount = loading.progress;
            yield return null;
        }
    }
}
