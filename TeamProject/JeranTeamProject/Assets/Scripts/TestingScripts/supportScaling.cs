using UnityEngine;


public class supportingNormalizeObjects : MonoBehaviour
{


    public void NormalizeParent(Transform parent)
    {
        parent.localScale = Vector3.one;
    }

    public void NormalizeChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Vector3 intendedScale = child.localScale;
            child.SetParent(null);
            child.localScale = intendedScale;
            child.SetParent(parent);
        }
    }

    public Vector3 NormalizeForScreen(Vector3 originalScale)
    {

        float refWidth = 1920f;
        float refHeight = 1080f;


        float widthRatio = Screen.width / refWidth;
        float heightRatio = Screen.height / refHeight;
        float scaleFactor = Mathf.Min(widthRatio, heightRatio);
        return originalScale * scaleFactor;
    }


}


