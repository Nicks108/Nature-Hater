using UnityEngine;
using System.Collections;

public class S_ProportionalPositionGUIElement : MonoBehaviour
{
    public Vector2 PositionPercentage;
    public int ZDepth;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.position = new Vector3(
            NTTools.NTUtils.Percentage(PositionPercentage.x, 1),
            NTTools.NTUtils.Percentage(PositionPercentage.y, 1),
            ZDepth);
	}
}
