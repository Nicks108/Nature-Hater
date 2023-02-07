using UnityEngine;
using System.Collections;

public class S_ScalGUIText : MonoBehaviour
{
    //public float Xscale=1;
    //public float Yscale = 1;

    public int fontScale =1
        ;

    void OnGUI()
    {
        //if (Xscale <= 0) Xscale = float.MinValue;
        //if (Yscale <= 0) Yscale = float.MinValue;

        //this.transform.localScale = new Vector3(Screen.width / Xscale, Screen.height / Yscale);
        if (fontScale <= 0) fontScale = 1;
        this.GetComponent<GUIText>().fontSize = Screen.width / fontScale;

    }
}
