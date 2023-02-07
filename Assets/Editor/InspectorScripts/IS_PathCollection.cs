using System.IO;
using UnityEngine;
using UnityEditor;
using NTTools;
using System.Collections.Generic;

[CustomEditor(typeof(S_PathCollection))]
public class IS_PathCollection : Editor
{
    private S_PathCollection _pathCollection;

    private static string DirectoryPath = "";
    private const string FilePath = "PathData.xml";
    private static string CurrentScene;
	// Use this for initialization
    private void OnEnable()
    {
        _pathCollection = (S_PathCollection) target;
        CurrentScene = EditorApplication.currentScene;
        if (DirectoryPath == "")
        {
            DirectoryPath = NTUtils.GameDataPath + CurrentScene + "/" + "Path Data";
        }
    }

    
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Load all paths"))
        {
            NTUtils.DeleteAllChildGameObjects(_pathCollection.gameObject);
            //string xml = Serialize.DeSerializeObject<string>(DirectoryPath + "/" + FilePath);
            UnityEngine.Debug.Log(DirectoryPath + "/" + FilePath);
            string xml = File.ReadAllText(DirectoryPath + "/" + FilePath);
            Debug.Log("loaded XML: "+ xml);
            List<S_Path.PathDataSerializer> PathScriptList = Serialize.SerializeFromXML<List<S_Path.PathDataSerializer>>(xml);
            //Debug.Log("PathscriptList.count: "+PathScriptList);
            foreach (S_Path.PathDataSerializer pathDataSerializer in PathScriptList)
            {
                GameObject pathObj = new GameObject();
                pathObj.transform.parent = _pathCollection.gameObject.transform;
                pathObj.AddComponent<S_Path>();
                S_Path pathScript = pathObj.GetComponent<S_Path>();
                pathObj.transform.position = pathDataSerializer.position.GetUnityVector3();
                //pathScript.Init(pathDataSerializer, pathObj);


                pathScript.Color = pathDataSerializer.Color;
                pathObj.name = pathDataSerializer.Name;
                pathScript.GUID = pathDataSerializer.myGuid;
                pathScript.PointSize = pathDataSerializer.PointSize;
                pathScript.isDebuging = pathDataSerializer.isDebuging;
                pathScript.isDrawSwitchRadei = pathDataSerializer.isDrawSwitchRadei;



                foreach (S_Path.PathDataSerializer.PathPointDataSerializer pathPointData in pathDataSerializer.PathPointDataList)
                {
                    GameObject point = new GameObject();
                    point.AddComponent<S_PathPoint>();
                    S_PathPoint pathPointScript = point.GetComponent<S_PathPoint>();
                    pathPointScript = new S_PathPoint(pathPointData);
                    point.transform.position = pathPointData.Position.GetUnityVector3();
                    point.name = pathPointData.Name;
                    pathScript.PathPoints.Add(pathPointScript);
                    point.transform.parent = pathObj.transform;
                }

                pathObj.tag = "Path";
                pathScript.UpdatePathPoint();
            }
        }
         if (GUILayout.Button("Save all paths"))
         {
             List<S_Path.PathDataSerializer> PathScriptList = new List<S_Path.PathDataSerializer>();
             foreach (GameObject path in GetAllPaths())
             {
                 S_Path.PathDataSerializer temp = new S_Path.PathDataSerializer(path.GetComponent<S_Path>());
                 PathScriptList.Add(temp);
             }
             string xml = Serialize.SerializeToXML(PathScriptList);
             UnityEngine.Debug.Log(DirectoryPath);
             Serialize.SerializeObject(xml, DirectoryPath, FilePath, true);
         }

        //if(GUILayout.Button("Clear all Event TODos"))
        //{
        //    GameObject[] pathArray = GameObject.FindGameObjectsWithTag("Path");
        //    foreach (GameObject path in pathArray)
        //    {
        //        foreach(S_EventToDo eventToDo in  path.GetComponents<S_EventToDo>())
        //        {
        //            DestroyImmediate(eventToDo);
        //        }
        //    }
        //}
         if (GUILayout.Button("move path point 0 to end to path point list"))
         {
             Debug.Log("begin");
             Debug.Log("parten name: " + _pathCollection.gameObject);
             foreach (Transform pathObj in _pathCollection.gameObject.transform)
             {
                 Debug.Log("moving");
                 S_Path pathScript = pathObj.gameObject.GetComponent<S_Path>();
                 S_PathPoint point0 = pathScript.PathPoints[0];
                 pathScript.PathPoints.RemoveAt(0);
                 pathScript.PathPoints.Add(point0);
             }

         }
    }

    // Update is called once per frame
	void Update () {
	
	}

    public GameObject[] GetAllPaths()
    {
        return GameObject.FindGameObjectsWithTag("Path");
    }
}
