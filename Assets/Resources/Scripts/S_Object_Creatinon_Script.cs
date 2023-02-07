using UnityEngine;
using System.Collections;

public class S_Object_Creatinon_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp()
	{
		GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
		g.transform.position = new Vector3(0,0,0);
		g.transform.localScale = new Vector3(20,10,32);
		//Debug.Log("WHERE MY CUBE!!!");
	}
	

    //IEnumerator OnMouseDown()
    void OnMouseDown()
    {
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "test.fbx");
        //Debug.Log(filePath);

        //if (filePath.Contains("://"))
        //{
            //WWW www = new WWW(filePath);
            //yield return www;
            //Debug.Log(www.text);
        //}
       // else
        //{
            //Debug.Log(System.IO.File.ReadAllText(filePath));
        //}

        //UnityEditor.AssetImporter assetImportes = new UnityEditor.AssetImporter();
        //assetImportes.
        //UnityEditor.ModelImporter mi = (UnityEditor.ModelImporter) 

        //GameObject i = Instantiate(Resources.Load("test", typeof(GameObject))) as GameObject;
        //i.transform.position = new Vector3(0, 0, 0);
        //i.transform.localScale = new Vector3(20,20,20);
    }
}
