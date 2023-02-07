//#define DEBUG
using System;
using UnityEngine;


namespace NTTools.Data
{
    [Serializable]
    public class Node
    {
        //public Node[] InputPositions;
        //public Node[] OutputPositons;
        public string Name;
        public Vector2 vec2Position;
        //bool isResizable = false;

        //public static Node selection = null;
        //public static bool connecting = false;
        public Rect nodeRect;
        public Rect _nodeRect
        {
            get { return nodeRect; }
            set { nodeRect = value; }
        }
        public Vector2 Center
        {
            get { return NTUtils.GetCenterOfRect(_nodeRect); }
        }

        public string GUID;

        public Node(Rect sizeAndPosition, string name)
        {
            GUID = NTUtils.GenerateGUID(this);
            this.Name = name;
            _nodeRect = sizeAndPosition;
        }
        public Node(Rect sizeAndPosition, string name, string guid)
        {
            GUID = guid;
            _nodeRect = sizeAndPosition;
        }
    }
}
