using UnityEngine;
using System.Collections;

public class S_CustomGuiRotatTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    //public Texture2D CenteralTexture;
    public Texture2D TextureToOrbit;
    public Rect rTextureToRotateRect;
    //public Vector2 szTextureOrbit;
    public Vector2 PivotPoint;
    public float Angle = 45;

    public Texture2D PivetPointDebugTexture;
    float PivetPointDebugTextureHight = 10f;
    float PivetPointDebugTextureWidth = 10f;

    public float Distance = 10f;

    void OnGUI()
    {
        //Rect rTextureOrbit = new Rect();
        Vector2 pivot = new Vector2();

        //set the pivot at the center of the centeral texture
        pivot.x = rTextureToRotateRect.x + rTextureToRotateRect.width /2 + PivotPoint.x;
        pivot.y = rTextureToRotateRect.y + rTextureToRotateRect.height / 2 + PivotPoint.y;

        //set the point rect at the circle top
        //rTextureOrbit.x = pivot.x - szTextureOrbit.x / 2;
        //rTextureOrbit.width = szTextureOrbit.x;
        //rTextureOrbit.y = rTextureToRotateRect.y - szTextureOrbit.y / 2;
        //rCenteralPoint.height = szTextureOrbit.y;
        float debugPivotPointX = pivot.x - PivetPointDebugTextureWidth/2;
        float debugPivotPointY = pivot.y - PivetPointDebugTextureHight/2;
        GUI.DrawTexture(new Rect(debugPivotPointX, debugPivotPointY, PivetPointDebugTextureWidth, PivetPointDebugTextureHight), PivetPointDebugTexture);
        //Matrix4x4 svMat = GUI.matrix;
        //GUIUtility.RotateAroundPivot(Angle % 360, pivot);
        //GUI.DrawTexture(rTextureToRotateRect, TextureToOrbit);
        //GUI.matrix = svMat;



        //float distance = 40f;
        //float NewAngle = Angle % 360;
        //Vector2 OldPosition = new Vector2(rTextureToRotateRect.x, rTextureToRotateRect.y);
        //Vector2 NewPositon = OldPosition + new Vector2(distance * Mathf.Cos(NewAngle) , distance *Mathf.Sin(NewAngle));

        Rect rNewRect = OrbitArountPoint2D(rTextureToRotateRect, new Vector2(debugPivotPointX, debugPivotPointY), Angle, Distance);

        GUI.DrawTexture(rNewRect, TextureToOrbit);
        Debug.Log("Orbit texture position X,Y: " + rTextureToRotateRect.x + "," + rTextureToRotateRect.y);
        Debug.Log("NewOrbit position X,Y: " + rNewRect);
    }

    public Rect OrbitArountPoint2D(Rect rObject, Vector2 v2CenterPoint, float fAngle, float fDistance)
    {
        float newAngle = fAngle % 360;
        float NewX = v2CenterPoint.x + fDistance * Mathf.Cos(newAngle);
        float NewY = v2CenterPoint.y + fDistance * Mathf.Sin(newAngle);
        NewX -= rObject.width / 2;
        NewY -= rObject.height / 2;
        return new Rect(NewX, NewY, rObject.width, rObject.height);
    }
}
