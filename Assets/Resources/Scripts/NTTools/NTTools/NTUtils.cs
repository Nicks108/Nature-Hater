using System;
using System.Collections.Generic;
//using UnityEditor;
using System.Linq;
using UnityEngine;

namespace NTTools
{
    public class NTUtils
    {
        public static string GenerateGUID<T>(T obj)
        {
            return obj.GetType().ToString() + ":" + Guid.NewGuid().ToString("N");
        }
        

        public static Quaternion RotateToFacePoint(Vector3 pointToFace, Transform ObjectTransform, float Offset = 0)
        {
            Vector3 result = pointToFace - ObjectTransform.position;

            float theta = Mathf.Atan2(result.y, result.x) * Mathf.Rad2Deg;
            theta += Offset;

            Vector3 temp = ObjectTransform.rotation.eulerAngles;
            temp.Set(temp.x, temp.y, theta);
            return Quaternion.Euler(temp);
        }

        public static T[] getAllChildObjects<T>(GameObject Perent)
        {
            List<T> list = new List<T>();
            foreach (Transform child in Perent.transform)
            {
                child.gameObject.GetComponent(typeof(T));
            }

            return list.ToArray();
        }

        public static Vector2 GetCenterOfRect(Rect r)
        {
            float centerX = r.x + (r.width / 2);
            float centerY = r.y + (r.height / 2);
            return new Vector2(centerX, centerY);
        }

        public static void DeleteAllChildGameObjects(GameObject GO)
        {
            foreach(Transform child in GO.transform) 
                GameObject.DestroyImmediate(child.gameObject);
        }

        public static  string ResorcePath
        {
            get { return Application.dataPath + "/Resources/"; }
        }
        public static string GameDataPath
        {
            get { return ResorcePath + "GameData/"; }
        }
        public static string GetGameDataFolderPathForCurrentScene(string CurrentScene)
        {
            return GameDataPath + "Assets/Levels/" + (CurrentScene).Substring((CurrentScene).LastIndexOf("/") + 1);
        }

        public static float FindAngle(Vector3 a, Vector3 b)
        {
            float angle = Mathf.Atan2(a.y - b.y, a.x - b.x);
            return angle;
        }

        public static Vector3 BallisticVel(Transform target, Transform initialpoint)
        {

            Vector3 dir = target.position - initialpoint.transform.position; // get target direction
            float h = dir.y;  // get height difference
            dir.y = 0;  // retain only the horizontal direction
            float dist = dir.magnitude;  // get horizontal distance
            dir.y = dist;  // set elevation to 45 degrees
            dist += h;  // correct for different heights
            float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
            return vel * dir.normalized;  // returns Vector3 velocity
        }
        public static float Percentage(float percentage, float value)
        {
            return (value/100)*percentage; 
        }

        public static Vector3 AverageVector3(List<Vector3> vectorList)
        {
            if (vectorList == null)
            {
                vectorList = new List<Vector3>();
            }
            return AverageVector3(vectorList.ToArray());
        }

        public static Vector3 AverageVector3(Vector3[] vectorArray)
        {
            Vector3 averageVec = Vector3.zero;
            foreach (Vector3 vector in vectorArray)
            {
                averageVec += vector;
            }

            return averageVec / vectorArray.Length;
        }
		
		 public static Rect GetAbsolutePositionFromPercentage(Rect percentageRect)
        {
            float left = Percentage(percentageRect.xMin, Screen.width);
            float top = Percentage(percentageRect.yMin, Screen.height);
            float width = Percentage(percentageRect.width, Screen.width);
            float hight = Percentage(percentageRect.height, Screen.height);

            return new Rect(left,top,width,hight);
        }

        public static int GetChildIndexByName(string Name, Transform transform)
        {
            int i = 0;
            for ( i=0; i< transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.name == Name)
                {
                    break;
                }
            }
            return i;
        }

        public static void SplitArrayAtIndex<T>(T[] array, int index, out T[] first, out T[] second)
        {
            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
        }
        public static void SplitArrayAtMidPoint<T>(T[] array, out T[] first, out T[] second)
        {
            NTUtils.SplitArrayAtIndex(array, array.Length / 2, out first, out second);
        }

        public static float SquaredEuclideanDistance(Vector3 a, Vector3 b)
        {
            float xSquare = (a.x - b.x)*(a.x - b.x);
            float ySquare = (a.y - b.y)*(a.y - b.y);
            float zSquare = (a.z - b.z)*(a.z - b.z);
            return xSquare + ySquare + zSquare;
        }
    }
}
