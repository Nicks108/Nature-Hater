using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace NTTools.NTGUI
{
    class NTTextBox : NTControle
    {
        public string Text;
        public string Lable;
        
        public NTTextBox()
        {

        }
        public NTTextBox(string lable, string text)
        {
            this.Lable = lable;
            this.Text = text;
        }

        public override void OnGUI()
        {
            if(Event.current.type == EventType.Repaint)
                Draw();
        }

        public override void Draw()
        {
            Text = EditorGUI.TextField(sizeAndPositonOfControle, Text, Text);
        }

        public override void DrawChildren()
        {
            foreach (var Child in ChildControles)
            {
                Child.Draw();
            }
        }
    }
}
