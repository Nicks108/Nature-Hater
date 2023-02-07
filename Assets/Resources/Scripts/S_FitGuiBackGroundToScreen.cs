using UnityEngine;
using System.Collections;

public class S_FitGuiBackGroundToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GUITexture GUIBackGround = this.GetComponent<GUITexture>();

        GUIBackGround.pixelInset= new Rect(GUIBackGround.pixelInset.xMin, GUIBackGround.pixelInset.yMin, Screen.width, Screen.height);
        //Debug.Log("Screen h, w:" + Screen.width+", "+ Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
        this.Start();
	}
}
