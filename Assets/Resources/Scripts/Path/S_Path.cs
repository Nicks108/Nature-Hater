using System;
using System.Collections.Generic;
using NTTools.Path;
using UnityEngine;


    [Serializable]
    public class S_Path : MonoBehaviour
    {

        public List<S_PathPoint> PathPoints = new List<S_PathPoint>();
        //public Color LineColor;
        //public Path PathData;

        public Color Color;

        public string Name
        {
            get { return this.gameObject.name; }
            set { this.gameObject.name = value; }
        }

        public string myGuid;

        public string GUID
        {
            get
            {
                if (myGuid == "")
                    myGuid = NTTools.NTUtils.GenerateGUID(this);
                return myGuid;
            }
            set { myGuid = value; }
        }

        public float PointSize = 0.5f;

        public bool isDebuging = false;
        public bool isDrawSwitchRadei = false;

        public S_Path()
        {
        }

        public void Init(PathDataSerializer path, GameObject parent)
        {
            Color = path.Color;
            Name = path.Name;
            GUID = path.myGuid;
            PointSize = path.PointSize;
            isDebuging = path.isDebuging;
            isDrawSwitchRadei = path.isDrawSwitchRadei;
            //this.gameObject.transform.position = path.position.GetUnityVector3();

            foreach (PathDataSerializer.PathPointDataSerializer pathPointData in path.PathPointDataList)
            {
                GameObject point = new GameObject();
                point.AddComponent<S_PathPoint>();
                S_PathPoint pathPointScript = point.GetComponent<S_PathPoint>();
                pathPointScript = new S_PathPoint(pathPointData);
                point.transform.position = pathPointData.Position.GetUnityVector3();
                point.name = pathPointData.Name;
                this.PathPoints.Add(pathPointScript);
                point.transform.parent = parent.transform;
            }

        }

        private void Awake()
        {
            //UnityEngine.Debug.Log("in path awake");
            //foreach (GameObject child in transform)
            //{
            //    S_PathPoint childScript = child.GetComponent<S_PathPoint>();

            //    PathPoints.Add(childScript);
            //}
        }

        private void AddPointToPath(Vector3 point)
        {
            //GameObject newPoint = new GameObject();
            //newPoint.name = "Point " + PathData.PathPoints.Count;
            //S_PathPoint pointScript = newPoint.AddComponent<S_PathPoint>();
            S_PathPoint newPoint = new S_PathPoint {Name = "Point " + PathPoints.Count};
            UnityEngine.Debug.Log("Ray hit");
            //newPos.z = 0;
            newPoint.transform.position = point;


            //GameObject go = new GameObject();
            //go.transform.position = point;
            PathPoints.Add(newPoint);
            //go.transform.parent = this.gameObject.transform;
        }

        public void CreateAndAddPathPointAtPoint(Vector3 point)
        {
            int index = 0;
            if (PathPoints.Count > 0)
                index = PathPoints.Count - 1;
            CreateAndAddPathPointAtIndexAtPoint(index, point);
        }

        public void CreateAndAddPathPointAtIndexAtPoint(int index, Vector3 point)
        {
            GameObject newPoint = new GameObject();
            //newPoint.name = "Point " + PathData.PathPoints.Count;
            S_PathPoint newPointScript = newPoint.AddComponent<S_PathPoint>();
            //S_PathPoint newPointScript = new S_PathPoint();
            newPointScript.Name = "Point " + PathPoints.Count;
            UnityEngine.Debug.Log("Ray hit");
            //newPos.z = 0;
            //newPoint.transform.position = point;
            newPoint.transform.position = point;
            newPoint.transform.parent = this.gameObject.transform;
            //Path.PathPoints.Add(pointScript);
            PathPoints.Insert(index, newPointScript);
            //newPoint.transform.parent = gameObject.transform;
        }

        public void drawLine()
        {
            //Debug.Log("draw line");
            if (PathPoints.Count < 1)
                throw new System.Exception("a path must contain atleased one point!");
            Vector3[] positionArray = new Vector3[PathPoints.Count];
            for (int i = 0; i < PathPoints.Count; i++)
            {
                positionArray[i] = PathPoints[i].transform.position;
                // Debug.Log("Path point: " + PathPoints[i].name);
                //Debug.Log("in loop");
                //Debug.Log(PathPoints[i].transform.position);
                //Debug.DrawLine(PathPoints[i-1].transform.position, PathPoints[i].transform.position,this.LineColor);
            }
            //todo why fix poly line draw in s_path

#if UNITY_EDITOR
            //Debug.Log("im debugging");

            Color oldColor = UnityEditor.Handles.color;
            Color.a = 1;
            UnityEditor.Handles.color = Color;
            UnityEditor.Handles.DrawPolyLine(positionArray);
            UnityEditor.Handles.color = oldColor;
#endif
        }

        public void Init()
        {
            //if (this.PathPoints.Count == 0)
            //{
            CreateAndAddPathPointAtPoint(new Vector3(2, 1.5f, 0));
            CreateAndAddPathPointAtPoint(new Vector3(-2, 1.5f, 0));
            //}
        }

        // Use this for initialization
        private void Start()
        {
            //if (LineColor == null)
            //{
            //    float R = Random.Range(0,100)/100;
            //    float G = Random.Range(0,100)/100;
            //    float B = Random.Range(0,100)/100;
            //    LineColor = new Color(R,G,B);
            //}
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void UpdatePathPoint()
        {
            PathPoints.Clear();
            foreach (Transform child in this.transform)
            {
                //Debug.Log("in loop");
                S_PathPoint childScript = child.gameObject.GetComponent<S_PathPoint>();

                PathPoints.Add(childScript);
                //Debug.Log(this.name +": "+ child.name);
            }
        }

        private void OnDrawGizmos()
        {
            //Debug.Log("draw gizmoz");
            if (isDebuging)
            {
                //Debug.Log("is debuging line");
                //Debug.Log("path points count: " + PathPoints.Count);
                if (PathPoints.Count < 1)
                {
                    //Debug.Log("count < 1");
                    UpdatePathPoint();
                }
                //Debug.Log("path points count: "+PathPoints.Count);
                if (PathPoints.Count < 1)
                    throw new System.Exception("a path must contain atleased one point!");
                //Debug.Log("PathPoints.Count: " + PathPoints.Count);
                for (int i = 1; i < PathPoints.Count; i++)
                {
                    //Debug.Log("i: " + i);
                    //Debug.Log("in line draw loop");
                    Gizmos.color = Color;
                    //Debug.Log("PathPoints[i-1].transform.position: " + PathPoints[i - 1].transform.position);
                    //Debug.Log("PathPoints[1].transform.position: " + PathPoints[1].transform.position);
                    Gizmos.DrawLine(PathPoints[i - 1].transform.position, PathPoints[i].transform.position);
                }
                foreach (S_PathPoint pathPoint in PathPoints)
                {
                    //Debug.Log("in circle draw loop");
                    //S_GameManager GameManager = GameObject.Find("GameManager").GetComponent<S_GameManager>();
                    Gizmos.color = Color;
                    //GameManager.TargetSwitchDistanceThreshold
                    Gizmos.DrawSphere(pathPoint.transform.position, PointSize);
                }
                drawLine();
            }
        }

        public void AddEventToObject(NTTools.Data.EventNode e)
        {
            this.gameObject.AddComponent<S_EventToDo>().SetUp(e);
        }


        public class PathDataSerializer
        {
            public Color Color;
            public string Name;
            public string myGuid;
            public float PointSize;
            public bool isDebuging;
            public bool isDrawSwitchRadei;
            public NTVector3 position;
            public List<PathPointDataSerializer> PathPointDataList;

            public PathDataSerializer(S_Path path)
            {
                Color = path.Color;
                Name = path.Name;
                this.myGuid = path.GUID;
                PointSize = path.PointSize;
                this.isDebuging = path.isDebuging;
                this.isDrawSwitchRadei = path.isDrawSwitchRadei;
                this.position = new NTVector3(path.gameObject.transform.position);

                PathPointDataList = new List<PathPointDataSerializer>();

                foreach (S_PathPoint point in path.PathPoints)
                {
                    PathPointDataList.Add(new PathPointDataSerializer(point.name, point.gameObject.transform.position,
                        point.PointType));
                }
            }

            public PathDataSerializer(Color color, string name, string myGuid, float pointSize, bool isDebuging,
                bool isDrawSwitchRadei, Vector3 position, List<S_PathPoint> pointList)
            {
                Color = color;
                Name = name;
                this.myGuid = myGuid;
                PointSize = pointSize;
                this.isDebuging = isDebuging;
                this.isDrawSwitchRadei = isDrawSwitchRadei;
                this.position = new NTVector3(position);

                foreach (S_PathPoint point in pointList)
                {
                    PathPointDataList.Add(new PathPointDataSerializer(point.name, point.gameObject.transform.position,
                        point.PointType));
                }
            }

            public PathDataSerializer()
            {
            }


            public class PathPointDataSerializer
            {
                public string Name;
                public NTVector3 Position;
                public S_PathPoint.PointTypes PointType = S_PathPoint.PointTypes.none;

                public PathPointDataSerializer()
                {
                }

                public PathPointDataSerializer(string name, Vector3 pos, S_PathPoint.PointTypes pointType)
                {
                    Name = name;
                    Position = new NTVector3(pos);
                    PointType = pointType;
                }
            }


        }
    }

