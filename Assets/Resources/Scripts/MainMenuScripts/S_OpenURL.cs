using UnityEngine;
using System.Collections;

public class S_OpenURL: MonoBehaviour
{

    public string url = "http://google.com/";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        Application.OpenURL(url);
    }
}
