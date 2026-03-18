using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    public static GameObject[] persistentObjects = new GameObject[3];
    public int persistentIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (persistentObjects[persistentIndex] == null)
        {
            persistentObjects[persistentIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[persistentIndex] != gameObject)
        {
            Destroy(gameObject);
        }

    }
   
}
