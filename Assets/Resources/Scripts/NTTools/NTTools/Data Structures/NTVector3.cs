using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NTVector3
{
    public float x;
    public float y;
    public float z;

    public NTVector3()
    {
    }
    public NTVector3(Vector3 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }


    public NTVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetUnityVector3()
    {
        return new Vector3(x,y,z);
    }

    public string ToString()
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
            sb.AppendFormat("{0}: {1}{2}", info.Name, value != null ? value : "null", Environment.NewLine);
        }

        return sb.ToString();
    }
}

