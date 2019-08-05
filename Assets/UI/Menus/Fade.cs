using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used for fading screen to/from black when switching scenes
/// </summary>
public class Fade : MonoBehaviour
{
  
    private static Animator Anim;
    
    /// <summary>
    /// Scene that will be loaded when fade out finishes
    /// </summary>
    private static string nextSceneName;

    
    private void Start()
    {
        Anim = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Starts fade to black animation, sets up next scene, possible to overwrite what happens at animation end after calling this
    /// </summary>
    /// <param name="_nextSceneName">Scene that will be loaded after fade out ends, overwrite onFadeFinished after calling this if not loading other scenes</param>
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
    /// Decides what happens when FadeToBlack animation ends, default: loads nextSceneName
    /// </summary>
    public static OnFadeFinished onFadeFinished;

}
