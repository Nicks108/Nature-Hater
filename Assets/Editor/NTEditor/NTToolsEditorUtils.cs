using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


namespace NTTools.NTEditor
{
    class NTToolsEditorUtils
    {
        public static void SetSelection(GameObject newSelection)
        {
            GameObject[] gos = new GameObject[1];
            gos[0] = newSelection;
            Selection.objects = gos;
        }
    }
}
