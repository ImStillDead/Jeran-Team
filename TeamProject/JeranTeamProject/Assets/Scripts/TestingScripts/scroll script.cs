using UnityEngine;
using UnityEngine.UI;

public class scrollscript : MonoBehaviour
{
    public Scrollbar bar;
    float scroll = Input.GetAxis("Mouse ScrollWheel");

    private void Update()
    {
        
        if(scroll > 0f)
        {
           bar.value= scroll;

        }
  


    }


}
