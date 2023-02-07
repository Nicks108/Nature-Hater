using UnityEngine;
using UnityEditor;

public class ES_NTToolsMainWindow : EditorWindow
{
    [MenuItem("Window/NTTool/Main Tools Window")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ES_NTToolsMainWindow));
    }

    void OnGUI()
    {
        NTTools.NTDebug.DebugEnabled = GUILayout.Toggle(NTTools.NTDebug.DebugEnabled, "Enable Debugging");
        NTTools.NTDebug.BreakPointsEnabled = GUILayout.Toggle(NTTools.NTDebug.BreakPointsEnabled, "Enable Brack points");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
