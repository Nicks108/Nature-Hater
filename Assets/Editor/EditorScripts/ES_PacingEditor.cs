using NTTools.Data;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using NTTools.NTGUI;


public class ES_PacingEditor : EditorWindow
{

    public Rect windowRect = new Rect(20, 20, 200, 150);
    public Rect ScrollRect = new Rect(10, 100, 500, 500);

    //public S_EventNodeList EventList;
    [MenuItem("Window/Pacing editor")]
    private static void Launch()
    {
         //Debug.Log("opening window");
         //Debug.Break();
        //GetWindow<PacingEditor>();//.title = "Pacing Editor";
        EditorWindow.GetWindow(typeof(ES_PacingEditor));
    }

    //private Rect[] _rectArray;
    //private EventNode[] _nodeArray;
    private List<GUIEventNode> guiEventList = new List<GUIEventNode>(); 
    void OnEnable()
    {
        buildGUIEventNodeList();

        EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
    }

    void buildGUIEventNodeList()
    {
        guiEventList.Clear();
        if (EventNode.EventList.Count >0)
        {
            foreach (EventNode E in EventNode.EventList)
            {
                GUIEventNode guievent = new GUIEventNode(E) {WindowID = EventNode.EventList.IndexOf(E)};
                guiEventList.Add(guievent);
                guievent.REFguiEventList = guiEventList;
            }
        }

        
    }

    void Update()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        {
            UnityEngine.Debug.Log("is compiling");
            EventNode.Save(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(EditorApplication.currentScene) + "/Pacing Data", "PacingEventData.xml");
            AssetDatabase.Refresh();
        }   
    }

    private Vector2 _scrollPosition;
    void OnGUI()
    {
        Repaint();
    }
    void Repaint()
    {

    //buildGUIEventNodeList();
        if (GUILayout.Button("Load Events"))
        {
            EventNode.Load(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(EditorApplication.currentScene) + "/Pacing Data");
            buildGUIEventNodeList();
        }
        if (GUILayout.Button("Save Events"))
        {
            //UnityEngine.Debug.Log(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(EditorApplication.currentScene)+ "Pacing Data");
            //C:\Users\Public\Documents\Unity Projects\nature-hater-1\Demo Level\NatureHater\Assets\Resources
            EventNode.Save(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(EditorApplication.currentScene) + "/Pacing Data", "PacingEventData.xml");
            AssetDatabase.Refresh();
        }
        //if(GUILayout.Button("Clear Leacks"))
        //{
        //    Resources.UnloadUnusedAssets();
        //}

        EventNode.isRepeat = GUILayout.Toggle(EventNode.isRepeat, "repeat these events?");
        if (GUILayout.Button("Clear Event Node list"))
        {
            EventNode.EventList.Clear();
            guiEventList.Clear();
        }

        Rect ScrollViewRect = new Rect(0, 0, 4000, 4000);
        ScrollRect = new Rect(0, 100, this.position.width - 10, this.position.height - 120);
        _scrollPosition = GUI.BeginScrollView(ScrollRect, _scrollPosition, ScrollViewRect, true, true);

        BeginWindows();
        
        foreach (GUIEventNode E in guiEventList)
        {
            //UnityEngine.Debug.Log("in draw loop, E.name: " + (E.Data as EventNode).NodeName + " nodeRext" + (E.Data as EventNode).NodeRect);
            E.Draw();
            E.OnGUI();
        }
        EndWindows();
        GUI.EndScrollView();
        //GUILayout.Button("this is a button");
        if (Event.current.type == EventType.mouseDown)
        {
            //UnityEngine.Debug.Log("mouse event " + Event.current.button);
            if (Event.current.clickCount == 2)
            {
                //UnityEngine.Debug.Log("main wind mouse down num clicks 2");
                if (GUIEventNode.Selection != null) return;

                Rect windowRect = new Rect(Event.current.mousePosition.x,
                    Event.current.mousePosition.y, 200, 150);
                EventNode eventNode = new EventNode(windowRect, "New Event");
                //GameObject.Find("PathCollection").GetComponent<S_EventNodeList>().EventList.Add(eventNode);
                EventNode.AddToEventList(eventNode);
                //UnityEngine.Debug.Log("EventNode.EventList.count: " + EventNode.EventList.Count);
                GUIEventNode guievent = new GUIEventNode(eventNode);
                guiEventList.Add(guievent);
                guievent.REFguiEventList = guiEventList;
                Event.current.Use();
                GUI.changed = true;
            }
        }

        if (GUI.changed)
        {
            //EditorUtility.SetDirty((UnityEngine.Object)GUIEventNode.EventList);
        }

    }


    //Delegates
    void OnPlaymodeStateChange()
    {
        Debug.Log("stateChanged");
        Resources.UnloadUnusedAssets();

        if (!EditorApplication.isPlaying)
        {
            EventNode.Save(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(EditorApplication.currentScene) + "/Pacing Data", "PacingEventData.xml");
            AssetDatabase.Refresh();
            GameObject.Find("PathCollection").GetComponent<S_SetUpPathEvents>().LoadEvents();
        }
        if (EditorApplication.isPaused)
        {

        }
        buildGUIEventNodeList();
        
    }
    
}
