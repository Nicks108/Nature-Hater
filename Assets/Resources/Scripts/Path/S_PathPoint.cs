using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public class S_PathPoint:MonoBehaviour
    {
        public string Name
        {
            get { return this.gameObject.name; }
            set { this.gameObject.name = value; }
        }
        //public Vector3 Position;

        public enum PointTypes
        {
            spawn = 0,
            none = 1,
            AttackPLayer = 2,
            Dive = 3,
            EndPoint = 4,
            ReturnToPool =5
        }
        public PointTypes PointType = PointTypes.none;

        public S_PathPoint()
        {
        }

        public  S_PathPoint(S_Path.PathDataSerializer.PathPointDataSerializer pathPointData)
        {
            this.PointType = pathPointData.PointType;
            //this.gameObject.transform.position = pathPointData.Position.GetUnityVector3();
            //this.Name = pathPointData.Name;
        }
    }
