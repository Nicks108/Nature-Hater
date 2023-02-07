using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NTTools.Data_Structures;
using NTTools;
using System;

[CustomEditor(typeof (S_EnameyPacing))]
public class IS_EnemeyPaceEditor : Editor
{
    private S_EnameyPacing EnemeyPacing;

    public bool isRepeating;

string DirectoryPath = "";
    string FilePath = "PacingEvents.xml";

    public override void OnInspectorGUI()
    {
        EnemeyPacing = (S_EnameyPacing)target;
        if (DirectoryPath == "")
        {
            DirectoryPath = Application.dataPath + "/Game Data/" + EditorApplication.currentScene + "/" + this.EnemeyPacing.gameObject.name;
        }
        

        ////EnemeyPacing.PaceEvents = Serialize.DeSerializeObject<List<NTEnamyPaceData>>(DirectoryPath +"/"+ FilePath);
        //if (EnemeyPacing.PaceEvents == null)
        //{
        //    EnemeyPacing.PaceEvents = new List<NTEnamyPaceData>();
        //}
        DrawLoadSaveButtons();
        
        AddNewEventButton();
        for (int i = 0; i < EnemeyPacing.PaceEvents.Count; i++)
        {
            NTEnamyPaceData data = EnemeyPacing.PaceEvents[i];
            if(i==0)
                data.isRepeat = GUILayout.Toggle(data.isRepeat, "Repeat?");
            
            if (GUILayout.Button("Move This Event Up"))
            {
                MoveEventUp(data);
                GUI.FocusControl("Create New Event");
            }
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name");
            data.NodeName = EditorGUILayout.TextField(data.NodeName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number Of Enemeys");
            data.NumberOfEnimeys = EditorGUILayout.IntSlider(data.NumberOfEnimeys, 1, 10);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time Between Spawns");
            data.TimeBetweenSpawns = EditorGUILayout.FloatField(data.TimeBetweenSpawns);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Wait Time Before Spawn Starts");
            data.WaitTimeBeforSpawnStarts = EditorGUILayout.FloatField(data.WaitTimeBeforSpawnStarts);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Move This Event down"))
            {
                MoveEventDown(data);
                GUI.FocusControl("Create New Event");
            }
            if (GUILayout.Button("Delete this Event"))
                EnemeyPacing.PaceEvents.Remove(data);
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(3) });
        }
        Repaint();
        //try
        //{
        //    Debug.LogError("EnemeyPacing " + EnemeyPacing.ToString());
        //    Serialize.SerializeObject(EnemeyPacing.PaceEvents, DirectoryPath, FilePath, true);
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError(ex);
        //}
    }

    private  void DrawLoadSaveButtons()
    {
        if (GUILayout.Button("Load Pace Data"))
            this._LoadPaceData();
        if(GUILayout.Button("Save Pace data"))
            this._savePaceData();
    }
    private void _LoadPaceData()
    {
        //List<NTEnamyPaceData> test = new List<NTEnamyPaceData>();
        EnemeyPacing.PaceEvents = Serialize.DeSerializeObject<List<NTEnamyPaceData>>(DirectoryPath + "/" + FilePath);
        if (EnemeyPacing.PaceEvents != null)
            isRepeating = EnemeyPacing.PaceEvents[0].isRepeat;
        //Debug.Log("print load test" + test.ToString());
    }
    private void _savePaceData()
    {
        try
        {
           // Debug.LogError("EnemeyPacing " + EnemeyPacing.ToString());
            Serialize.SerializeObject(EnemeyPacing.PaceEvents, DirectoryPath, FilePath, true);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void MoveEventUp(NTEnamyPaceData dataToMove)
    {
        int indexOfCurrentData = EnemeyPacing.PaceEvents.IndexOf(dataToMove);

        if (indexOfCurrentData <= 0)
            return;

        indexOfCurrentData --;
        EnemeyPacing.PaceEvents.Remove(dataToMove);
        EnemeyPacing.PaceEvents.Insert(indexOfCurrentData, dataToMove);
    }
    public void MoveEventDown(NTEnamyPaceData dataToMove)
    {
        int indexOfCurrentData = EnemeyPacing.PaceEvents.IndexOf(dataToMove);

        if (indexOfCurrentData >= EnemeyPacing.PaceEvents.Count - 1)
            return;

        indexOfCurrentData++;
        EnemeyPacing.PaceEvents.Remove(dataToMove);
        EnemeyPacing.PaceEvents.Insert(indexOfCurrentData, dataToMove);
    }

    public void AddNewEventButton()
    {
        if(GUILayout.Button("Create New Event"))
        {
            EnemeyPacing.PaceEvents.Add(new NTEnamyPaceData("node NAme", 1, 0f, 0f, isRepeating));
        }
    }

    public new string ToString()
    {
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy;
        System.Reflection.PropertyInfo[] infos = this.GetType().GetProperties(flags);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string typeName = this.GetType().Name;
        sb.AppendLine(typeName);
        sb.AppendLine(string.Empty.PadRight(typeName.Length + 5, '='));

        foreach (var info in infos)
        {
            object value = info.GetValue(this, null);
            sb.AppendFormat("{0}: {1}{2}", info.Name, value != null ? value : "null", Environment.NewLine);
        }

        return sb.ToString();
    }
}
