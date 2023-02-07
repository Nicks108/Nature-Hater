using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace NTTools.NTGUI
{
    class NTButton : NTControle
    {
        public string Text;
        public delegate void ButtonPressed();

        public ButtonPressed OnButtonPressed;

        public NTButton()
        {

        }

        public NTButton(ref ButtonPressed d)
        {
            OnButtonPressed += d;
        }

        public override void OnGUI()
        {
            Draw();
        }

        public override void Draw()
        {
            if (GUILayout.Button(Text))
            {
                if (OnButtonPressed != null)
                    OnButtonPressed();
            }
        }

        public override void DrawChildren()
        {

        }
    }
}
