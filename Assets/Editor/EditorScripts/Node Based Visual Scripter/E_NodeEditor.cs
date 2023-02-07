using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using System.Linq;

namespace NTTools.NodeScripter
{
    public class E_NodeEditor : EditorWindow
    {

        List<Node> nodes = new List<Node>();

        [MenuItem("Window/Node Scripter")]
        static void Launch()
        {
            GetWindow<E_NodeEditor>().title = "Example";
        }

        void OnGUI()
        {
            GUILayout.Label("Node Scripter");
            if (GUILayout.Button("Delete all nodes"))
            {
                nodes.Clear();
            }
            
            //render all connections first
            if (Event.current.type == EventType.repaint)
            {
                foreach (Node node in nodes)
                {
                    foreach (Node target in node.Targets)
                    {
                        Node.DrawConnecton(node.Position, target.Position);
                    }
                }
            }
            GUI.changed = false;

            //handel all nodes
            BeginWindows();
            foreach (Node node in nodes)
            {
                //Debug.Log("draw nodes");
                node.OnGUI();
            }
            EndWindows();
            wantsMouseMove = Node.Selection != null;
            //if we have a selection, we're doing an opperation which requiers an update each mouse move

            switch (Event.current.type)
            {
                case EventType.mouseUp:
                    Node.Selection = null;
                    Event.current.Use();
                    break;
                case EventType.mouseDown:
                    UnityEngine.Debug.Log("mouse down");
                    if (Event.current.clickCount == 2)
                    {
                        UnityEngine.Debug.Log("click count " + Event.current.clickCount);
                        Node.Selection = new Node("Node " + nodes.Count, nodes.Count, Event.current.mousePosition);
                        nodes.Add(Node.Selection);
                        Event.current.Use();
                        GUI.changed = true;
                    }
                    break;
            }

            if (GUI.changed)
            {
                UnityEngine.Debug.Log("repaint");
                Repaint();
            }
        }
    }

    public class Node
    {
        public Node[] InputPositions;
        public Node[] OutputPositons;

        Vector2 position;
        const float Nodesize = 50f;
        //bool isResizable = false;

        public static Node selection = null;
        public static bool connecting = false;

        Rect NodeRect;
        string Name;
        int ID;
        List<Node> targets = new List<Node>();

        public Node(string name, int id, Vector2 pos)
        {
            this.Name = name;
            Position = pos;
            ID = id;

            InputPositions = new Node[1];
            OutputPositons = new Node[1];

        }
        public static Node Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                if (selection == null)
                {
                    connecting = false;
                }
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                NodeRect = new Rect(
                    position.x - Nodesize * 0.5f,
                    position.y - Nodesize * 0.5f,
                    Nodesize,
                    Nodesize);
            }
        }
        public ReadOnlyCollection<Node> Targets
        {
            get { return targets.AsReadOnly(); }
        }

        public void ConnectTo(Node target)
        {
            if (OutputPositons.Contains(target))
                return;
            //if (target.InputPositions
            //targets.Add(target);
            //target.InputPositions
        }

        public void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    if (NodeRect.Contains(Event.current.mousePosition))
                    {
                        selection = this;

                        if (Event.current.clickCount == 2)
                        {
                            connecting = true;
                        }
                        Event.current.Use();
                    }
                    break;
                case EventType.mouseUp:
                    if (selection == null)
                    {
                        break;
                    }
                    else if (selection == this)
                    {
                        if (!connecting)
                        {
                            Selection = null;
                            Event.current.Use();
                        }
                    }
                        //if connecting and over another node
                    else if (connecting && NodeRect.Contains(Event.current.mousePosition))
                    {
                        if (!(this is ES_VeriableNode))
                            //if target node is not of type veriable
                        {
                            selection.ConnectTo(this);
                        }
                            Selection = null;
                        Event.current.Use();
                    }
                    break;
                case EventType.mouseDrag:
                    if (selection == this)
                    {
                        if (connecting)
                        {
                            Event.current.Use();
                        }
                        else
                        {
                            Position += Event.current.delta;
                            Event.current.Use();
                        }
                    }
                    break;
                case EventType.repaint:
                    UnityEngine.Debug.Log("Node repaint");
                    UnityEngine.Debug.Log("Rect details: " + NodeRect);
                    //GUI.skin.box.Draw(NodeRect, new GUIContent(Name), false, false,false,false);

                    DrawNode();

                    if (selection == this && connecting)
                    {
                        GUI.color = Color.red;
                        DrawConnecton(position, Event.current.mousePosition);
                        GUI.color = Color.white;
                    }
                    break;
            }
        }


        public void DrawNode()
        {
            GUI.Window(ID, NodeRect, DrawNodeWindow, Name);
            
            foreach(Node InputPosition in InputPositions  )
            {
                float newX = (NodeRect.x) - 5;
                float newY = (NodeRect.y + (NodeRect.height /(InputPositions.Length+1)));
                Rect attachmentPoint = new Rect(newX, newY, 5,5);
                GUI.Box(attachmentPoint, "Text");
            }

            foreach (Node OutPosition in OutputPositons)
            {
                float newX = NodeRect.x + NodeRect.width;
                float newY = NodeRect.y + (NodeRect.height /(InputPositions.Length+1)) ;
                Rect attachmentPoint = new Rect(newX, newY, 5, 5);
                GUI.Box(attachmentPoint, "Text");
            }
        }

        void DrawControles()
        {

        }

        void DrawNodeWindow(int id)
        {
            //Rect buttonRect = new Rect(NodeRect.xMin+2, NodeRect.yMin+5, NodeRect.width-10, NodeRect.height - 10);
            //GUI.Button(buttonRect, "button");
            GUI.DragWindow();
        }

        public static void DrawConnecton(Vector2 from, Vector2 to)
        {
            bool left = from.x > to.x;
            left = false;//from always on left, to always on right

            //Texture2D lineTexture = new Texture2D(1, 2);
            //Color transparentWhite = new Color(1, 1, 1, 0);
            //Color opaqueWhite = Color.blue;
            //lineTexture.SetPixel(0, 0, transparentWhite);
            //lineTexture.SetPixel(0, 1, opaqueWhite);
            //lineTexture.Apply();

            Handles.DrawBezier(
                new Vector3(from.x + (left ? -Nodesize : Nodesize) * 0.5f, from.y, 0.0f),
                new Vector3(to.x + (left ? Nodesize : -Nodesize) * 0.5f, to.y, 0.0f),
                new Vector3(from.x, from.y, 0.0f) + Vector3.right * 100.0f * (left ? -1.0f : 1.0f),
                new Vector3(to.x, to.y, 0.0f) + Vector3.right * 100.0f * (left ? 1.0f : -1.0f),
                GUI.color,
                null,
                2.0f
            );

        }
    }

    class NodeConnectionPoint
    {
        Node refConnectedNode;
        Rect ConnectionRect;

        //float size =5;

        Node Perant;

        public NodeConnectionPoint(Node parent)
        {
            Perant = parent;
        }

        public void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    //size = 10;
                    break;
                case EventType.mouseUp:
                    if (Node.Selection == this.Perant)
                    {
                        if (!Node.connecting)
                        {
                            Node.Selection = null;
                            Event.current.Use();
                        }
                    }
                    //if connecting and over another node
                    else if (Node.connecting && ConnectionRect.Contains(Event.current.mousePosition))
                    {
                        if (!(this.Perant is ES_VeriableNode))
                        //if target node is not of type veriable
                        {
                            Node.selection.ConnectTo(this.Perant);
                        }
                        Node.Selection = null;
                        Event.current.Use();
                    }
                    break;
                //case EventType.mouseDrag:
                //    if (Node.selection == this.Perant)
                //    {
                //        if (Node.connecting)
                //        {
                //            Event.current.Use();
                //        }
                //        else
                //        {
                //            Position += Event.current.delta;
                //            Event.current.Use();
                //        }
                //    }
                //    break;
                //case EventType.repaint:
                //    Debug.Log("connector repaint");
                //    Debug.Log("connector Rect details: " + ConnectionRect);
                //    //GUI.skin.box.Draw(NodeRect, new GUIContent(Name), false, false,false,false);

                //    this.Perant.DrawNode();

                //    if (Node.selection == this.Perant && Node.connecting)
                //    {
                //        GUI.color = Color.red;
                //        DrawConnecton(position, Event.current.mousePosition);
                //        GUI.color = Color.white;
                //    }
                //    break;
            }
        }
    }
}
