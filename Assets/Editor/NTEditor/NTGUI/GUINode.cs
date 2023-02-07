//#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.ObjectModel;
using NTTools.Data;

namespace NTTools.NTGUI
{
    public class GUINode :NTControle
    {

        //public Node[] InputPositions;
        //public Node[] OutputPositons;

        public Vector2 vec2Position;
        //bool isResizable = false;

        public static GUINode selection = null;
        public static bool connecting = false;

        [SerializeField]
        protected string WindowTitle = "";

        public Node Data;

        public Rect NodeRect
        {
            get
            {
                //UnityEngine.Debug.Log("NodeRect: "+ Data._nodeRect);
                return Data._nodeRect;
            }
            set { Data._nodeRect = value; }
        }
        public Vector2 Center
        {
            get { return NTUtils.GetCenterOfRect(NodeRect); }
        }

        [SerializeField] 
        public int _windowID = -1;
        public int WindowID
        {
            get 
            { 
                if(_windowID == -1)
                {
                    _windowID = EventNode.EventList.IndexOf(this.Data as EventNode);
                }
                return _windowID;
            }
            set { _windowID = value; }
        }
        public static GUINode Selection
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
            get { return vec2Position; }
            set
            {
                vec2Position = value;
                Data._nodeRect = new Rect(
                    Data._nodeRect.x - Data._nodeRect.width * 0.5f,
                    Data._nodeRect.y - Data._nodeRect.height * 0.5f,
                    Data._nodeRect.width,
                    Data._nodeRect.height);
            }
        }
        public GUINode()
        {
            
        }
        public GUINode(Node data)
        {
            Data = data;
            this.WindowTitle = Data.Name;

            RepaintMe += new RepaintHandel(RepaintMefunc);

            MouseUp += new MouseUpHandle(MouseUpFunc);
            MouseDown += new MouseDownHandle(MouseDownFunc);
            MouseDrag += new MouseDragHandle(MouseDragFunc);
        }

        public void Draw(GUI.WindowFunction func)
        {
            //UnityEngine.Debug.Log("Wndow ID: " + this.WindowID);
            //UnityEngine.Debug.Log("NodeRect:  " + NodeRect);
            //UnityEngine.Debug.Log("this.WindowTitle: " + this.WindowTitle);
            NodeRect = GUI.Window(this.WindowID, NodeRect, func, this.WindowTitle);
        }

        

        public override void Draw()
        {
            Draw(DrawWindow);
        }


        public void DrawWindow(int windowID)
        {
            DrawChildren();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        public override void DrawChildren()
        {
            foreach (var Child in ChildControles)
            {
                Child.Draw();
            }
        }
        

        public static GUINode _lastNodeClicked;
        public override void OnGUI()
        {
            //bool childControleContainsMousePos =
                //this.ChildControleContains(Event.current.mousePosition);

            
            switch (Event.current.type)
            {
                case EventType.mouseDown:
#if DEBUG
                        {
                            UnityEngine.Debug.Log("MouseDown" + this.name);
                        }
#endif
                        //if(childControleContainsMousePos)break;
                        _lastNodeClicked = this;
                        //MouseDown(this, e);
                        OnMouseDown(e);
                    break;
                case EventType.mouseUp:
                        //if (childControleContainsMousePos) break;
                        //MouseUp(this, e);
                        OnMouseUp(e);
#if DEBUG
                        {
                            UnityEngine.Debug.Log("MouseUp");
                        }
#endif
                        _lastNodeClicked = null;
                    break;
                case EventType.mouseDrag:
                        //if (childControleContainsMousePos) break;
                        //MouseDrag(this, e);
                        OnMouseDrag(e);
#if DEBUG
                        {
                            UnityEngine.Debug.Log("MouseDrag");
                        }
#endif
                    break;
                case EventType.Repaint:
#if DEBUG
                    {
                        UnityEngine.Debug.Log("repaint");
                    }
#endif
                    OnRepaint(e);
                    break;




            }
        }
        //private int innerController = 0;
        /// <summary>
        /// this function makes unity shut up about null referances on OnGUI() call
        /// </summary>
        private void MouseUpFunc(NTControle c, EventArgs ea)
        {
            
        }
        private void MouseDownFunc(NTControle c, EventArgs ea)
        {
            

        }
        private void MouseDragFunc(NTControle c, EventArgs ea)
        {
#if DEBUG
UnityEngine.Debug.Log("mouse dragging");
#endif
        }
        private void RepaintMefunc(NTControle c, EventArgs ea)
        {
            //base.Repaint();
            //UnityEngine.Debug.Log("repainting");
            //_nodeRect = GUI.Window(ID, _nodeRect, DrawWindow, this.Name);
        }

    }
}
