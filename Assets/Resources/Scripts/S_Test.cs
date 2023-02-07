using UnityEngine;
using System.Collections;

public class S_Test : MonoBehaviour {

    public GameObject spline;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log(spline.ToString());
        Debug.Log("vertex count: "+spline.GetComponent<MeshFilter>().mesh.vertexCount);
        Debug.Log("Vertex's: "+spline.GetComponent<MeshFilter>().mesh.vertices);
    }
}
