using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using NTTools.Data;


namespace NTTools.NTGUI
{
    [System.Serializable]
    public class GUIEventNode : GUINode
    {
        [SerializeField] //private static S_EventNodeList _eventNodeList;
            public static List<EventNode> EventList;
        //public EventNode Data;
        //public static bool isRepeat;
        //public static List<S_Path> PathLIst = new List<S_Path>(); 

        private static Texture _ArrowLeft;
        private static Texture _ArrowRight;


        public GameObject[] EnemyPoolArray;

        //[SerializeField]
        //public List<EventNode> DependantsList = new List<EventNode>();
        //[SerializeField]
        //public List<EventNode> DependancyList = new List<EventNode>(); 

        public GUIEventNode(EventNode data) 
        {
            //Data.NodeName = data.NodeName;
            //Data.NumberOfEnimeys = data.NumberOfEnimeys;
            //Data.WaitTimeBeforSpawnStarts = data.WaitTimeBeforSpawnStarts;
            //Data.TimeBetweenSpawns = data.TimeBetweenSpawns;
            //Data.Speed = data.Speed;
            //Data.PathGUID = data.PathGUID;
            //Data.HasSpawnFinished = data.HasSpawnFinished;
            Data = data;
            //UnityEngine.Debug.Log("data.NodeRect: " + data.NodeRect);
            //UnityEngine.Debug.Log("Data.NodeRect: " + (Data as EventNode).NodeRect);
            Init();
        }

        public void Init()
        {
            (Data as EventNode).IsStartPoint = false;
            //EventList.Add(this);

            //GameObject[] GOList = GameObject.FindGameObjectsWithTag("Path");
            //PathLIst.Clear();
            //for (int i = 0; i < GOList.Length; i++)
            //{
            //    var go = GOList[i];
            //    PathLIst.Add(go.GetComponent<S_Path>());
            //}

            setupArrow();
            setupDelegates();

            EnemyPoolArray = GameObject.FindGameObjectsWithTag("EnemyPool");
        }
        void setupArrow()
        {
            if (_ArrowLeft == null)
            {
                _ArrowLeft = Image.GetSingleImage(Application.dataPath + "\\Resources\\Textures\\AimArrowHead Left.png");
            }
            if (_ArrowRight == null)
            {
                _ArrowRight = Image.GetSingleImage(Application.dataPath + "\\Resources\\Textures\\AimArrowHead Right.png");
            }
        }

        void setupDelegates()
        {
            this.MouseDown -= mouseDownEvent;
            this.MouseDown += mouseDownEvent;
            this.MouseUp -= mouseUpEvent;
            this.MouseUp += mouseUpEvent;
            this.RepaintMe -= Repaint;
            this.RepaintMe += Repaint;
        }

        public static new bool connecting = false;
        [SerializeField]
        private int pathIndex = 0;
        public new void DrawWindow(int WindowID)
        {
            GameObject[] pathArray = GameObject.FindGameObjectsWithTag("Path");
            string[] PathNames = new string[pathArray.Length];
            for (int i = 0; i < pathArray.Length; i++)
                PathNames[i] = pathArray[i].name;

            foreach (GameObject pathOBJ in pathArray)
            {
                if (pathOBJ.GetComponent<S_Path>().GUID == (Data as EventNode).PathGUID)
                    pathIndex = pathArray.ToList().IndexOf(pathOBJ);
            }

            //this.name = GUILayout.TextField(this.name);
            (Data as EventNode).NodeName = EditorGUILayout.TextField("Name", (Data as EventNode).NodeName);
            this.WindowTitle = (Data as EventNode).NodeName;
            (Data as EventNode).NumberOfEnimeys = (int)EditorGUILayout.Slider("Number of Enamies", (Data as EventNode).NumberOfEnimeys, 1f, 20f);
            (Data as EventNode).Speed = EditorGUILayout.Slider("Speed", (Data as EventNode).Speed, 0, 10);
            (Data as EventNode).TimeBetweenSpawns = EditorGUILayout.Slider("Time Between Spawns", (Data as EventNode).TimeBetweenSpawns, 0f, 10f);
            (Data as EventNode).WaitTimeBeforSpawnStarts = EditorGUILayout.Slider("Wait Time befor start", (Data as EventNode).WaitTimeBeforSpawnStarts, 0f, 10f);
            


            this.pathIndex = EditorGUILayout.Popup(pathIndex, PathNames);
            (Data as EventNode).PathGUID = pathArray[pathIndex].GetComponent<S_Path>().GUID;

            List<string> EnemyPoolNames = new List<string>();
            foreach (GameObject go in EnemyPoolArray)
            {
                EnemyPoolNames.Add(go.name);
            }

            int poolIndex =0;
            for (int index = 0; index < EnemyPoolArray.Length; index++)
            {
                GameObject EnemyObj = EnemyPoolArray[index];
                if (EnemyObj.GetComponent<S_EnemyPool>() == (Data as EventNode).EnemyPoolRef)
                    poolIndex = index;
            }
            poolIndex = EditorGUILayout.Popup(poolIndex, EnemyPoolNames.ToArray());
            (Data as EventNode).EnemyPoolRef =EnemyPoolArray[poolIndex].GetComponent<S_EnemyPool>();

            base.DrawWindow(WindowID);
           
        }
        public override void Draw()
        {
            //UnityEngine.Debug.Log("Event list count: " + EventList.Count);
            //int c = GameObject.Find("PathCollection").GetComponent<S_EventNodeList>().Count;
            //UnityEngine.Debug.Log("count from game object: "+c);
            //UnityEngine.Debug.Log("Event list instance ID: "+ EventList.GetInstanceID());
            //UnityEngine.Debug.Log("Event list Gameobject instance ID: " + GameObject.Find("PathCollection").GetComponent<S_EventNodeList>().GetInstanceID());
            //UnityEngine.Debug.Log("Dependancy count: " + DependancyList.Count);
            //UnityEngine.Debug.Log("Dependant count: "+DependantsList.Count);


            // this stuff is setup on draw as it is lost on serialization. fucking annoying and there must be a better way to do it. perhaps look for on level load and or game start
            setupArrow();
            setupDelegates();
            Draw(DrawWindow);
        }

        public void Draw(GUI.WindowFunction func)
        {
            //UnityEngine.Debug.Log("Wndow ID: " + this.WindowID);
            //UnityEngine.Debug.Log("NodeRect:  " + NodeRect);
            //UnityEngine.Debug.Log("this.WindowTitle: " + this.WindowTitle);
            Color backGround = Color.white;
            switch ((Data as EventNode).EventState)
            {
                case EventNode.EventStates.inEditor:
                    backGround = Color.white;
                    break;
                case EventNode.EventStates.spawnFInished:
                    backGround = Color.red;
                    break;
                case EventNode.EventStates.spawning:
                    backGround = Color.green;
                    break;
                case EventNode.EventStates.waiting:
                    backGround = Color.yellow;
                    break;
            }

            GUI.backgroundColor = backGround;
            NodeRect = GUI.Window(this.WindowID, NodeRect, func, this.WindowTitle);
            GUI.backgroundColor = Color.gray;
        }

        

        public void mouseUpEvent(NTGUI.NTControle c, EventArgs e)
        {
            //UnityEngine.Debug.Log("in mouse up event");
            if(this.NodeRect.Contains(Event.current.mousePosition))
            {
                if(GUIEventNode.connecting && GUIEventNode.Selection != this)
                {
                    //UnityEngine.Debug.Log("in mouse up event Connecting");
                    (Data as EventNode).DependancyList.Add(GUIEventNode.selection.Data as EventNode);
                    //(GUIEventNode.Selection.Data as EventNode).DependantsList.Add(this.Data as EventNode);
                    //UnityEngine.Debug.Log("dependant list size "+ this.DependsOnEventList.Count.ToString());
                }
                GUIEventNode.Selection = null;
                GUIEventNode.connecting = false;
                Event.current.Use();
            }
        }

        public void mouseDownEvent(NTGUI.NTControle c, EventArgs e)
        {
            if(this.NodeRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.button == 0)
                {
                    selection = this;
                    //UnityEngine.Debug.Log("in mouse down event");
                    if (Event.current.clickCount == 2)
                    {
                        //UnityEngine.Debug.Log("in mouse down event 2 clicks");
                        GUIEventNode.connecting = true;
                    }
                }
                else if(Event.current.button == 1)
                {
                    //UnityEngine.Debug.Log("right click");
				    GenericMenu menu = new GenericMenu ();

                    menu.AddItem(new GUIContent("Delete"), false, CallbackDeleteNode, "Delete");
				    //menu.AddItem (new GUIContent ("MenuItem2"), false, Callback, "item 2");
            	    //menu.AddSeparator ("");
				    //menu.AddItem (new GUIContent ("SubMenu/MenuItem3"), false, Callback, "item 3");
				
				    menu.ShowAsContext ();
                }
            }
        }
        public void CallbackDeleteNode (System.Object obj)
        {
            DeleteNode();
            DeleteNode();
        }

        public List<GUIEventNode> REFguiEventList;
        protected void DeleteNode()
        {
            //UnityEngine.Debug.Log ("Selected: " + obj);
            EventNode.EventList.Remove(this.Data as EventNode);
            foreach (var E in EventNode.EventList)
            {
                //if (E.DependantsList.Contains(this.Data as EventNode))
                    //E.DependantsList.Remove(this.Data as EventNode);
                if (E.DependancyList.Contains(this.Data as EventNode))
                    E.DependancyList.Remove(this.Data as EventNode);
            }

            REFguiEventList.Remove(this);
        }

        public void Repaint(NTGUI.NTControle c, EventArgs e)
        {
            //UnityEngine.Debug.Log("in Repaint event");
            if (GUIEventNode.selection == this && GUIEventNode.connecting)
            {
                //UnityEngine.Debug.Log("in Repaint event draring line");
                GUI.color = Color.red;
                //Vector2 pos = new Vector2(_nodeRect.xMin, _nodeRect.yMin);
                GUIEventNode.DrawLine(this.Data.Center, Event.current.mousePosition);
                GUI.color = Color.white;
            }
            //draw connection to the node this one depends on
            //foreach (var dependant in from dependancy in (Data as EventNode).DependantsList let dependancyPos = new Vector2(dependancy.NodeRect.x, dependancy.NodeRect.y) select dependancy)
            //{
            //    GUIEventNode.DrawLine(,this.Center, dependant.Center);
            //    //UnityEngine.Debug.Log("center: "+ this.Center +", noderect: "+ this.NodeRect);
            //    GUIEventNode.DrawArrow(this.Center, dependant.Center, this.NodeRect.width);
            //}
            foreach (EventNode Dependancy in (Data as EventNode).DependancyList)
            {
                GUIEventNode.DrawLine(NTUtils.GetCenterOfRect(Dependancy.NodeRect),this.Center);
                GUIEventNode.DrawArrow(NTUtils.GetCenterOfRect(Dependancy.NodeRect), this.Center, this.NodeRect.width);
            }
        }

        public static void DrawLine(Vector2 from, Vector2 to)
        {
            List<Vector3> Points = new List<Vector3>();

            Points.Add(from);
            float xMidPoint = from.x + ((to.x - from.x) / 2); // X point inbetween these 2 points
            //UnityEngine.Debug.Log("xMidPoint: " + xMidPoint);
            Vector3 midPointTop = new Vector3(xMidPoint, from.y);
            Points.Add(midPointTop);
            //UnityEngine.Debug.Log("midpointTop: "+ midPointTop);

            Vector3 midPointBotom = new Vector3(xMidPoint, to.y);
            Points.Add(midPointBotom);
            //UnityEngine.Debug.Log("midPointBotom: " + midPointBotom);

            Points.Add(to);

            Handles.DrawPolyLine(Points.ToArray());
        }

        public static void DrawArrow(Vector2 from, Vector2 to, float NodeWidth)
        {
            float size = 20;
            bool left = from.x < to.x;
            float XPos = 0;
            Texture arrow;
            if (left)
            {
                XPos = to.x - (NodeWidth / 2) - size;
                arrow = GUIEventNode._ArrowLeft;
            }
            else
            {
                XPos = to.x + (NodeWidth/2);
                arrow = GUIEventNode._ArrowRight;
            }
            Rect posAndSize = new Rect(XPos, to.y - (size/2), size, size);

            GUI.color = Color.red;
            //GUI.Box(posAndSize,arrow);
            GUI.DrawTexture(posAndSize, arrow);
            GUI.color = Color.white;
        }

        //public override void Draw()
        //{
        //    //this.name = EditorGUILayout.TextField("Name", this.name);
        //    Rect positonOfControle = new Rect(5,10,20,20);
        //    this.name = EditorGUI.TextField(positonOfControle, "Name", this.name);
        //    GUI.DragWindow(new Rect(0, 0, 10000, 20));
            
        //}
        
        /// <summary>
        /// toString via class reflection
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy;
            System.Reflection.PropertyInfo[] infos = this.GetType().GetProperties(flags);

            StringBuilder sb = new StringBuilder();

            string typeName = this.GetType().Name;
            sb.AppendLine(typeName);
            sb.AppendLine(string.Empty.PadRight(typeName.Length + 5, '='));

            foreach (var info in infos)
            {
                object value = info.GetValue(this, null);
                sb.AppendFormat("{0}: {1}{2}", info.Name, value ?? "null", Environment.NewLine);
            }

            return sb.ToString();
        }

        

        
    }
}
