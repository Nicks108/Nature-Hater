using UnityEngine;
using System.Collections;
using NTTools;

[RequireComponent (typeof(GUITexture))]
public class S_ScaleTexture : MonoBehaviour
{
    public GUITexture texture;
    public Vector2 Scale;
	// Use this for initialization
	void Start () {
        texture = this.gameObject.GetComponent<GUITexture>();
	}
	
	// Update is called once per frame
	void Update () {
        Rect Temp =  new Rect( texture.transform.position.x,texture.transform.position.y,
            NTUtils.Percentage(Scale.x, Screen.width), NTUtils.Percentage(Scale.y , Screen.height));
        this.gameObject.GetComponent<GUITexture>().pixelInset = Temp;
	}
}
