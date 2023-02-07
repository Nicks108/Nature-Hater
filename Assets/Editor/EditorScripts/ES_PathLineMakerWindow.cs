using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using NTTools.Path;

public class ES_PathLineMakerWindow : EditorWindow {

    //bool CreateNewPathBool = false;
    bool StartNewPath = true;

    //bool EnableXAxisForNewPathpoint = true;
    //bool EnableYAxisForNewPathpoint = true;
    //bool EnableZAxisForNewPathpoint = true;

    GameObject Path;

    [MenuItem("Window/Path Maker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ES_PathLineMakerWindow));
    }

    void OnGUI()
    {
        if(GUILayout.Button("Load New Path from file"))
        {
            LoadAndDrawPathFromObj();   
        }
        if (GUILayout.Button("Create New Path"))
        {
            //if no path collection
            //
            GameObject PathCollection = GameObject.Find("PathCollection"); 
            if (PathCollection == null)
            {
                PathCollection = new GameObject("PathCollection");
                PathCollection.AddComponent<S_PathCollection>();
            }

            //create new path
            GameObject NewPath = new GameObject("Path");
            //add Path script
            NewPath.AddComponent<S_Path>();
            NewPath.GetComponent<S_Path>().Init();
            //Debug.Log("after init");
            NewPath.tag = "Path";
            NewPath.GetComponent<S_Path>().GUID = NTTools.NTUtils.GenerateGUID(NewPath.GetComponent<S_Path>());
            //NewPath.AddComponent<S_EnameyPacing>();
            //NewPath.AddComponent<S_PathSpawner>();
            //select new line
            GameObject[] gos = new GameObject[1];
            gos[0] = NewPath;
            Selection.objects = gos;
            NewPath.transform.parent = PathCollection.transform;
        }
    }

    public void Update()
    {

        //GameObject[] TargetPointerList = GameObject.FindGameObjectsWithTag("Path");
        //foreach (GameObject GO in TargetPointerList)
        //{
        //    S_Path script = GO.GetComponent<S_Path>();
        //    script.drawLine();
        //    //foreach (GameObject point in script.PathPoints)
        //    //{
        //    //    //Debug.Log("drawing path");
        //    //    Debug.DrawLine(GO.transform.position, point.transform.position);
        //    //}
        //    //Debug.Log(script.TargetPathNode.name);
        //}

    }
    public void OnSceneGUI()
    {
        
    }

    void LoadAndDrawPathFromObj()
    {
        string FileLocation = EditorUtility.OpenFilePanel("Load Path file", Application.streamingAssetsPath + "..//", "obj");
        //Debug.Log("file location: "+ FileLocation);
        if (FileLocation.Length != 0)
        {
            WWW www = new WWW("file:///" + FileLocation);
            while (!www.isDone)//wait for www to finish downloading. why dont i just read the file ye old'e fasiond way???
            {
            }
            string fileText = www.text;
            //Debug.Log(fileText);

            string[] lines = fileText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            //Debug.Log(lines.GetLength(0));
            List<Vector3> verteses = new List<Vector3>();
            foreach (string line in lines)
            {
                //Debug.Log(line);
                if (line.StartsWith("v"))
                {
                    //Debug.Log("starts with a v");
                    string tempText = line.Trim();
                    tempText = tempText.Remove(0, 2);
                    //Debug.Log("temp text after trim: " + tempText);
                    string[] vertsAsText = line.Split(new string[] { " " }, StringSplitOptions.None);
                    foreach (string s in vertsAsText)
                    {
                        Debug.Log(s);
                    }
                    //Debug.Log("vertAsText Length: "+vertsAsText.Length);
                    //Debug.Log(vertsAsText);

                    bool AllOk = true;
                    float x;
                    if (!float.TryParse(vertsAsText[2], out x))
                        AllOk = false;
                    float y;
                    if (!float.TryParse(vertsAsText[3], out y))
                        AllOk = false;
                    float z;
                    if (!float.TryParse(vertsAsText[4], out z))
                        AllOk = false;

                    if (AllOk)
                    {
                        Vector3 point = new Vector3(x, y, z);
                        Debug.Log(point);
                        verteses.Add(point);
                    }
                }
            }

            if (verteses.Count > 0)
            {
                GameObject Path = new GameObject("Path01");
                Path.AddComponent<S_Path>();
                Path.transform.position = new Vector3(0, 0, 0);
                Path.tag = "Path";
                S_Path pathScript = Path.GetComponent<S_Path>();

                //foreach (Vector3 point in verteses)
                    //pathScript.AddPointToPath(point);

                pathScript.drawLine();
            }
        }
    }

    private void CreatePathPoint(Vector3 point)//this is stupid, do it inthe fing class!!!
    {
        if (StartNewPath == true)
        {
            Path = new GameObject();
            Path.transform.position = point;
            Path.AddComponent<S_Path>();
            
            StartNewPath = false;
        }
        //on click in editor window, create new path point.
    }
    private void CreatePathPoint()
    {
        CreatePathPoint(new Vector3(0,0,0));        
    }

}
