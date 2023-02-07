using UnityEngine;
using System.Collections;

public class OpenUrl : MonoBehaviour {
    public string url = "http://google.com/";
    void OnMouseUp()
    {
        Application.OpenURL(url);
    }
}
