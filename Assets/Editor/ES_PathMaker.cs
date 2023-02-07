using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(S_Player))]
public class ES_PathMaker : UnityEditor.Editor {

    public override void OnInspectorGUI()
    {
        S_Player test = (S_Player)target;
        test.Scalefactor = EditorGUILayout.IntSlider("test in slider", System.Convert.ToInt32(test.Scalefactor), 0, 10);
    }
}
