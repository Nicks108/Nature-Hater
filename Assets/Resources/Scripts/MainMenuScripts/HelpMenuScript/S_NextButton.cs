using UnityEngine;
using System.Collections;

public class S_NextButton : MonoBehaviour
{
    public MeshRenderer background;
    public int materialIndex = 0;
    public Material[] Materials;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnMouseDown()
    {
        materialIndex ++;
        background.material = Materials[materialIndex%Materials.Length];
    }
}
