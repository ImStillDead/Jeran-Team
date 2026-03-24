using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class aisle : MonoBehaviour
{
    [Header("objects")]
    [SerializeField] List<TMP_Text> sign;
    [SerializeField] List<Image> backGround;

    [Header("settings")]
    [SerializeField] int aisleNumber;
    public Color32 on;
    public Color32 off;
    public bool isActive;

    private void Start()
    {
        for (int i = 0; i < sign.Count; i++)
        {
            sign[i].text = aisleNumber.ToString();
        }

        signActive();

    }

    private void signActive()
    {


       for(int i = 0;i < backGround.Count; i++)
        {
            if (!isActive)
            {
                backGround[i].color = off;
                Debug.Log("sign is not active");

            }

            else if(isActive)
            {
                backGround[i].color = on;
                Debug.Log("sign is active");
            }

        }
    }


}
