using UnityEngine;
using System.Collections;

public class S_RotateGUIEliment : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float rotAngle = 0;
    public Vector2 pivotPoint;
    void OnInspecterGUI()
    {}
    void OnGUI()
    {
        pivotPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
        if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50), "Rotate"))
            rotAngle += 10;

    }
}
