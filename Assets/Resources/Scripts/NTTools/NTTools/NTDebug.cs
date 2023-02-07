using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools
{
    public class NTDebug //: MonoBehaviour
    {
        public static bool DebugEnabled;
        public static bool BreakPointsEnabled;
        string OwnerObjectName;
        GameObject gameObject;
        
        public NTDebug(GameObject parent)
        {
            OwnerObjectName = parent.name;
            gameObject = parent;
            S_DebugScript debugscript = this.gameObject.GetComponent<S_DebugScript>();
            if (debugscript == null)
                this.gameObject.AddComponent<S_DebugScript>();
        }

        public void Log(string Input)
        {
            bool isDebugging = this.gameObject.GetComponent<S_DebugScript>().isDebugging;

            if (isDebugging)
                UnityEngine.Debug.Log(OwnerObjectName + ": " + Input);
        }

        public void Break()
        {
            bool isBreaking = this.gameObject.GetComponent<S_DebugScript>().isBreaking;
            if (isBreaking)
                UnityEngine.Debug.Break();
        }

    }
}
