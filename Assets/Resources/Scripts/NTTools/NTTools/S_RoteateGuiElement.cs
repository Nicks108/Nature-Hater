using UnityEngine;
using System.Collections;

public class S_RoteateGuiElement : MonoBehaviour
{
    public Matrix4x4 Rotation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        //THIS DOSNT WORK
	    Matrix4x4 Original = GUI.matrix;
        GUI.matrix = Rotation; 
	    GUI.matrix = Original;
	}
}
