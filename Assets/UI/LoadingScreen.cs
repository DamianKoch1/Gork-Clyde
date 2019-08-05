using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    public static string NextLevelName;


    [SerializeField]
    private Image loadingBar;

    private bool started;
   
    private void Start()
    {
        started = false;
    }

    private void Update()
    {
        if (!started)
        {
            //if called from start, loading screen itself will take too long to be loaded
            StartCoroutine(LoadNextLevel());
        }
    }

    /// <summary>
    /// Loads next level async, shows progress on bar
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextLevel()
    {
        started = true;
        AsyncOperation loading = SceneManager.LoadSceneAsync(NextLevelName);
        loading.allowSceneActivation = false;
        loadingBar.fillAmount = 0;
        while (loading.progress < 0.9f)
        {
            loadingBar.fillAmount = loading.progress;
            yield return null;
        }
        loadingBar.fillAmount = loading.progress;
        Fade.FadeToBlack();
        Fade.onFadeFinished = () => { loading.allowSceneActivation = true; };
    }

   
}
