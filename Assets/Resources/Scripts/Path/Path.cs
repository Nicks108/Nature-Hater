using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools.Path
{
    public class Path
    {
        //public static List<Path> PathList;

        public Color Color;
        public string Name;
        public string GUID;
        public List<S_PathPoint> PathPoints;
        public float PointSize = 0.5f;

        public Path(string name, Color color, List<S_PathPoint> pathPoints)
        {
            Color  = color;
            Name = name;
            PathPoints = pathPoints;
            GUID = NTUtils.GenerateGUID("Path:");
        }
        public Path(string name, Color color, List<S_PathPoint> pathPoints, string guid)
        {
        GUID = guid;
        Color = color;
        Name = name;
        PathPoints = pathPoints;
        }

        public void AddPathPoint()
        {
            
        }

    }
}
