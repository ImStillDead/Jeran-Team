using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static GameManager gameManagerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

  
}
