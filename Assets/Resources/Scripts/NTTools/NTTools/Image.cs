using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NTTools
{
    public class Image
    {
        //load images
        //animation
        public static List<Texture> GetAllFramesForAnimation(string path, string fileType)
        {
            //Debug.Log("Loading from: " + path);
            List<Texture> AnimationCollection = new List<Texture>();
            string[] Files = Directory.GetFiles(path, fileType);

            //Debug.Log(Files.Length);

            foreach (string file in Files)
            {
                AnimationCollection.Add(GetSingleImage(file));
            }
            return AnimationCollection;
        }
        //static
        public static Texture GetSingleImage(string path, string fileType)
        {
            return GetSingleImage(path + fileType);
        }
        public static Texture GetSingleImage(string FileAndType)
        {
            //Debug.Log("Loading from: " + path);

            WWW www = new WWW("file://" + FileAndType);
            while (!www.isDone)
            {
                //Debug.Log("loading");
            }
            //Debug.Log("Load compleat: " + file);
            return www.texture;
        } 
    }
}
