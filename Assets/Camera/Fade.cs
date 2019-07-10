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
    
    public static void FadeToBlack()
    {
        Anim.SetTrigger("fadeToBlack");
    }

    public void OnBlackFadeFinished()
    {
        SceneManager.LoadScene(NextSceneName);
    }

}
