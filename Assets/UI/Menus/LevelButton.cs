using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] 
    private int levelId;
    
    // Start is called before the first frame update
    void Start()
    {
        if (levelId > GameSaver.HighestLevelId)
        {
            Destroy(gameObject);
        }
    }

}
