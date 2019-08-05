using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
  
    private static Animator Anim;
    private static string nextSceneName;


    
    private void Start()
    {
        Anim = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Starts fade to black animation
    /// </summary>
    public static void FadeToBlack(string _nextSceneName = "")
    {
        nextSceneName = _nextSceneName;
        Anim.SetTrigger("fadeToBlack");
        onFadeFinished = () => 
        { 
            SceneManager.LoadScene(nextSceneName); 
        };
    }

 
    /// <summary>
    /// Calls delegate, is called when FadeToBlack animation ends
    /// </summary>
    public void OnBlackFadeFinished()
    {
        onFadeFinished();
    }


    public delegate void OnFadeFinished();

    /// <summary>
    /// Decides what happens when FadeToBlack animation ends, default: loads next scene
    /// </summary>
    public static OnFadeFinished onFadeFinished;

}
