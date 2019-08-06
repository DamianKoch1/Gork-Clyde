using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    
    /// <summary>
    /// Scene that will be loaded
    /// </summary>
    public static string NextLevelName;


    [SerializeField]
    private Image loadingBar;

    //-----------
    //if called from start, loading screen itself will take too long to be loaded
    private bool started = false;

    private void Update()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(LoadNextLevel());
        }
    }
    //-----------

    
    /// <summary>
    /// Loads next level async, shows progress on bar, fades out when done
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextLevel()
    {
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
