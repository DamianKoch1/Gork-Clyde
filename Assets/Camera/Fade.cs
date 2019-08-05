using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
  
    private static Animator Anim;
    public static string NextSceneName;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Starts fade to black animation
    /// </summary>
    public static void FadeToBlack()
    {
        Anim.SetTrigger("fadeToBlack");
    }

    /// <summary>
    /// loads given scene when fade is finished
    /// </summary>
    public void OnBlackFadeFinished()
    {
        LoadingScreen.NextLevelName = NextSceneName;
        SceneManager.LoadScene("Loading Screen");
    }

}
