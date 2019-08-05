using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] 
    private int levelId;
    
    void Start()
    {
        CheckIfUnlocked();
    }

    /// <summary>
    /// Deletes itself when saved highest level id is lower than this buttons levelId
    /// </summary>
    private void CheckIfUnlocked()
    {
        if (levelId > GameSaver.HighestLevelId)
        {
            Destroy(gameObject);
        }
    }

}
