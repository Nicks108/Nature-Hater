using NTTools.Path;
using UnityEngine;
using System.Collections;
using UnityEditor;
using NTTools;

[CustomEditor(typeof (S_Path))]
public class IS_PathEditor : Editor
{
    private S_Path Path;
    private bool isAddingPoints = false;

    private void OnSceneGUI()
    {
        Path = (S_Path) target;
        //Debug.Log("in on screen GUI");
        if (isAddingPoints)
        {
            //Debug.Log("Adding new points");
            //Debug.Log("current event "+ Event.current.ToString());
            if (Event.current.type == EventType.used)
            {
                //Debug.Log("im here!!");
            }
            if (Event.current.type == EventType.mouseUp)
            {
                //Debug.Log("mouse up event");
                //Vector3 newPos = Camera.main.ScreenPointToRay(Event.current.mousePosition).origin;
                //Vector3 newPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(10f);
                RaycastHit hitinfo;
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitinfo))
                {
                    ((S_Path) target).CreateAndAddPathPointAtPoint(hitinfo.point);
                    //Debug.Log("Mouse button pressed");
                    //NTTools.NTUtils.SetSelection(Path.gameObject);
                    //Debug.Log(Camera.current.ScreenPointToRay(Event.current.mousePosition));

                    EditorUtility.SetDirty(Path);
                }
                Event.current.Use();
            }
            //Catches bug in OnSceanGUI where mouse up is not detected for left mouse button.
            if (Event.current.type == EventType.layout)
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }
        //Debug.Log("on scean event cust editor");
        // Path.drawLine();
    }

    private string tempGUID;
    private void OnEnable()
    {
        Path = (S_Path)target;
        tempGUID = Path.GUID;
    }



    public override void OnInspectorGUI()
    {
        
        if (GUILayout.Button("force Path point update"))
            Path.UpdatePathPoint();

        
        GUI.SetNextControlName("txtGUID");
        tempGUID =EditorGUILayout.TextField("GUID: ", tempGUID);

        if (tempGUID != Path.GUID)
        {
            GUILayout.BeginHorizontal();
            GUI.color = Color.green;
            if (GUILayout.Button("Apply GUI Change"))
                Path.GUID = tempGUID;
            GUI.color = Color.red;
            if (GUILayout.Button("Cancel"))
                tempGUID = Path.GUID;
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        //if (GUILayout.Button("Generate New GUID"))
        //    Path.GUID = NTUtils.GenerateGUID(Path);

        Path.Color =EditorGUILayout.ColorField("Line Color",Path.Color);

        string EditButtonText;
        if (isAddingPoints)
            EditButtonText = "Disable Add New Points";
        else
            EditButtonText = "Enable Add New Points";

        if (GUILayout.Button(EditButtonText))
        {
            isAddingPoints = !isAddingPoints;
        }

        Path.isDebuging = GUILayout.Toggle(Path.isDebuging, "Debug?");
        if(Path.isDebuging)
        {
            GUILayout.Label("Debug point size: "+Path.PointSize);
            Path.PointSize = GUILayout.HorizontalSlider(Path.PointSize, 0.01f, 0.3f);
        }
        //Debug.Log(Path.PathPoints.Count);

        //DrawDefaultInspector();
        //Debug.Log("Path: "+ Path.PathPoints.Count);
        if (Path.PathPoints.Count > 0)
        {
            for (int i = 0; i < Path.PathPoints.Count; i++)
            {
                //S_PathPoint point = Path.PathData.PathPoints[i];
                S_PathPoint point = Path.PathPoints[i];
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Index in list: " + Path.PathPoints.IndexOf(point));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                if (i != 0 || i == Path.PathPoints.Count - 1)
                {
                    if (GUILayout.Button("Delete"))
                    {
                        Path.PathPoints.Remove(point);
                        DestroyImmediate(point.gameObject);
                        return; //supresses "collection modifiyed during loop, error
                    }
                    if (GUILayout.Button("Insert Above"))
                    {
                        Vector3 minusPositon = point.transform.position - Path.PathPoints[i - 1].transform.position;
                        Vector3 NewPositon = point.transform.position - minusPositon;
                        ((S_Path)target).CreateAndAddPathPointAtIndexAtPoint(i, NewPositon);
                        return; //supresses "collection modifiyed during loop, error
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                point.name = EditorGUILayout.TextField(point.name);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                if(i == 0)
                {
                    point.PointType = S_PathPoint.PointTypes.spawn;
                    EditorGUILayout.LabelField("Spawn point");
                }
                else if (i == Path.PathPoints.Count - 1)//the last node
                {
                    if (point.PointType != S_PathPoint.PointTypes.ReturnToPool && point.PointType != S_PathPoint.PointTypes.AttackPLayer && point.PointType != S_PathPoint.PointTypes.EndPoint)
                        point.PointType = S_PathPoint.PointTypes.ReturnToPool;

                    point.PointType = (S_PathPoint.PointTypes)EditorGUILayout.EnumPopup(point.PointType);
                    EditorGUILayout.LabelField("end point, return to pool");
                }
                else
                {
                    point.PointType = (S_PathPoint.PointTypes) EditorGUILayout.EnumPopup(point.PointType);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        //DrawDefaultInspector();
        
    }



}
