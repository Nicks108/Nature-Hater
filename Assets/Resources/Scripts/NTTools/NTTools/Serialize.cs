using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace NTTools
{
    public class Serialize
    {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string directory, string fileName, bool CreatePath = false)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(directory +"/"+ fileName))
            {
                File.Create(directory + "/" + fileName);
            }
            if (serializableObject == null) {
                return;
            }
            UnityEngine.Debug.Log(directory+"/"+fileName);

#if UNITY_WEBPLAYER
            //TODO in web player write lines (needed for game or just editor? prob just editor)
#else
            File.WriteAllText(directory +"/"+ fileName, (serializableObject as string));
#endif
            //

            //BinaryFormatter ser = new BinaryFormatter();
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    ser.Serialize(ms, serializableObject);
            //    byte[] bytes = ms.ToArray();

            //    try
            //    {
            //        // Open file for reading
            //        System.IO.FileStream _FileStream =
            //           new System.IO.FileStream(directory+"/"+fileName, System.IO.FileMode.Create,
            //                                    System.IO.FileAccess.Write);
            //        // Writes a block of bytes to this stream using data from
            //        // a byte array.
            //        _FileStream.Write(bytes, 0, bytes.Length);
            //        // close file stream
            //        _FileStream.Close();
            //    }
            //    catch (Exception _Exception)
            //    {
            //        // Error
            //        throw(_Exception);
            //    }
            //}

            //try
            //{
            //    XmlDocument xmlDocument = new XmlDocument();
            //    XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        serializer.Serialize(stream, serializableObject);
            //        stream.Position = 0;
            //        xmlDocument.Load(stream);
            //        xmlDocument.Save(fileName);
            //        stream.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw(ex);
            //}
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            UnityEngine.Debug.Log("fileName "+fileName);
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            
            T objectOut = default(T);
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fileName);
                //byte[] bytes = new byte[fs.Length];
                //fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                UnityEngine.Debug.Log("File tream \n"+fs.ToString());
                BinaryFormatter bin = new BinaryFormatter();
                objectOut = (T)bin.Deserialize(fs);
               
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            return objectOut;
        }

        public static T SerializeFromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static string SerializeToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                string StrippedXml = stringWriter.ToString();
                //StrippedXml = Regex.Replace(StrippedXml, @"[^a-zA-Z0-9]", string.Empty);

                UnityEngine.Debug.Log(StrippedXml.Trim());
                return StrippedXml.Trim();
            }
        }
    }
}
