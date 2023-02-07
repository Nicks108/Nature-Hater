using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Xml.Serialization;
using NTTools.Data_Structures;

public class S_EnameyPacing : MonoBehaviour {

    public List<NTEnamyPaceData> PaceEvents = new List<NTEnamyPaceData>();
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public new string ToString()
    {
        string result = "";

        result += "PaceEvents count: " +PaceEvents.Count + "\n";

        foreach(NTEnamyPaceData data in PaceEvents)
        {
            result += data.ToString()+"\n";
        }

        return result;
    }
}
