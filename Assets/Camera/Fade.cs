using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
  
    private static Animator ANIM;
    public static string NEXT_SCENE_NAME;

    private void Start()
    {
        ANIM = GetComponent<Animator>();
    }
    
    public static void FADE_TO_BLACK()
    {
        ANIM.SetTrigger("fadeToBlack");
    }

    public void OnBlackFadeFinished()
    {
        SceneManager.LoadScene(NEXT_SCENE_NAME);
    }

}
