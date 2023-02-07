using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class S_CameraFlip : MonoBehaviour
{
    public bool FlipX;
    public bool FlipY;
    public bool FlipZ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPreCull ()
    {
        float x = 1;
        float y = 1;
        float z = 1;
        if (FlipX)
            x = -1;
        if (FlipY)
            y = -1;
        if (FlipZ)
            z = -1;
        Vector3 flipMatrix = new Vector3(x,y,z);
	    camera.ResetWorldToCameraMatrix ();
	    camera.ResetProjectionMatrix ();
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(flipMatrix);
    }
 
    void OnPreRender () {
	    GL.SetRevertBackfacing (true);
    }
 
    void OnPostRender () {
	    GL.SetRevertBackfacing (false);
    }
}
