using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTTools.NTGUI{
    [System.Serializable]
    public abstract class NTControle: ScriptableObject
    {
        [SerializeField]
        protected Rect rectPosition;
        [SerializeField]
        protected List<NTControle> ChildControles = new List<NTControle>(); 
        //Events
        [SerializeField]
        public event MouseUpHandle MouseUp;
        [SerializeField]
        public event MouseDownHandle MouseDown;
        [SerializeField]
        public event MouseDragHandle MouseDrag;
        [SerializeField]
        public event RepaintHandel RepaintMe;
        [SerializeField]
        public delegate void MouseUpHandle(NTControle c, EventArgs e);
        [SerializeField]
        public delegate void MouseDownHandle(NTControle c, EventArgs e);
        [SerializeField]
        public delegate void MouseDragHandle(NTControle c, EventArgs e);
        [SerializeField]
        public delegate void RepaintHandel(NTControle c, EventArgs e);

        public EventArgs e = new EventArgs();

        [SerializeField]
        public Rect sizeAndPositonOfControle = new Rect(5, 20, 40, 20);


        public NTControle[] GetControles()
        {
            return ChildControles.ToArray();
        }
        public void AddControle(NTControle c)
        {
            ChildControles.Add(c);
        }

        public abstract void OnGUI();
        public abstract void Draw();
        public abstract void DrawChildren();

        protected virtual void OnMouseUp(EventArgs e)
        {
            MouseUpHandle handler = MouseUp;
            if(handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnMouseDown(EventArgs e)
        {
            MouseDownHandle handler = MouseDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnMouseDrag(EventArgs e)
        {
            MouseDragHandle handler = MouseDrag;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnRepaint(EventArgs e)
        {
            RepaintHandel handler = RepaintMe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected bool Contains(Vector2 p)
        {
            return this.rectPosition.Contains(p);
        }
        protected bool ChildControleContains(Vector2 p)
        {
            bool reslut = false;
             foreach (NTControle childControle in ChildControles)
            {
                if (childControle.Contains(p))
                {
                    reslut = true;
                    break;
                }
            }
            return reslut;
        }
    }
}
