using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools.Data
{
    [Serializable]
    public class EventNode : Node
    {
        //[SerializeField] //private static S_EventNodeList _eventNodeList;
            //private static List<EventNode> _eventList; 
        public static List<EventNode> EventList
        {
            get
            {
                //UnityEngine.Debug.Log("Eventlist.count: " + _eventList.Count);
                //if(_eventList == null)
                //    _eventList = new List<EventNode>();
                return  GameObject.Find("PathCollection").GetComponent<S_EventNodeList>().EventNodeList;
            }
            set { GameObject.Find("PathCollection").GetComponent<S_EventNodeList>().EventNodeList = value; }
        }
        public static void AddToEventList(EventNode e)
        {
            //UnityEngine.Debug.Log("adding object to eventlist: "+e.NodeName);
            //UnityEngine.Debug.Log("eventlist.count " + EventList.Count); 
            List<EventNode> tempEventList = EventList;
            tempEventList.Add(e);
            EventList = tempEventList;
            //UnityEngine.Debug.Log("eventlist.count after " + EventList.Count); 
        }

        public static bool isRepeat;
        //public static List<S_Path> PathLIst = new List<S_Path>(); 

        ///private static Texture _ArrowLeft;
        // private static Texture _ArrowRight;

        public Rect NodeRect
        {
            get { return _nodeRect; }
            set { _nodeRect = value; }
        }

        public string _nodeName;
        public string NodeName
        {
            get
            {
                return _nodeName;
            }
            set 
            { 
                _nodeName = value;
                Name = value;
            }
        }
        public int NumberOfEnimeys = 1;
        public float WaitTimeBeforSpawnStarts = 0;
        public float TimeBetweenSpawns = 0;
        public float Speed = 0.5f;

        //public S_Path Path;
        public string PathGUID;

        public S_EnemyPool EnemyPoolRef;

        public bool HasSpawnFinished = false;
        public bool IsStartPoint = false;

        //public List<EventNode> DependantsList = new List<EventNode>();
        public List<EventNode> DependancyList = new List<EventNode>();


        public enum EventStates
        {
            inEditor = 0,
            spawning = 1,
            waiting = 2,
            spawnFInished = 3,
        }

        public EventStates _eventState = EventStates.inEditor;
        public EventStates EventState
        {
            get
            {
                if (!Application.isPlaying)
                    return _eventState = EventStates.inEditor;
                else
                    return _eventState;

            }
            set { _eventState = value; }
        }


        public EventNode(Rect sizeAndPosition, string name)
            : base(sizeAndPosition, name)
        {
            this.NodeName = name;
            Init();
        }

        public EventNode(Rect sizeAndPosition, string name, string guid, string nodeName, int numberOfEnimeys,
                         float waitTimeBeforSpawnStarts, float timeBetweenSpawns, float speed, string pathGuid,
                         bool hasSpawnFinished, string EnemypoolName)
            : base(sizeAndPosition, name, guid)
        {
            NodeName = nodeName;
            NumberOfEnimeys = numberOfEnimeys;
            WaitTimeBeforSpawnStarts = waitTimeBeforSpawnStarts;
            TimeBetweenSpawns = timeBetweenSpawns;
            Speed = speed;
            PathGUID = pathGuid;
            HasSpawnFinished = hasSpawnFinished;


            GameObject[] pools = GameObject.FindGameObjectsWithTag("EnemyPool");
            foreach (GameObject go in pools)
            {
                if (go.name == EnemypoolName)
                    this.EnemyPoolRef = go.GetComponent<S_EnemyPool>();
            }
            
            Init();
        }

        public void Init()
        {
            IsStartPoint = false;
        }

        /// <summary>
        /// toString via class reflection
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.FlattenHierarchy;
            System.Reflection.PropertyInfo[] infos = this.GetType().GetProperties(flags);

            StringBuilder sb = new StringBuilder();

            string typeName = this.GetType().Name;
            sb.AppendLine(typeName);
            sb.AppendLine(string.Empty.PadRight(typeName.Length + 5, '='));

            foreach (var info in infos)
            {
                object value = info.GetValue(this, null);
                sb.AppendFormat("{0}: {1}{2}", info.Name, value ?? "null", Environment.NewLine);
            }

            return sb.ToString();
        }






        public static Dictionary<EventNode, string[]> EventAndItsConnectonsDic =
            new Dictionary<EventNode, string[]>();

        [SerializeField] public static List<SaveEventData> EventsToSave = new List<SaveEventData>();

        public static string[] GetConnectedDependancyGUIDs(EventNode thisEvent)
        {
            //return thisEvent.DependantsList.Select(e => e.GUID).ToArray();
            return thisEvent.DependancyList.Select(e => e.GUID).ToArray();
        }

        public static void BuildEventsToSaveList()
        {
            EventsToSave.Clear();
            foreach (EventNode e in EventNode.EventList)
            {
                EventsToSave.Add(new SaveEventData(e.GUID, e.NodeName, e.NumberOfEnimeys, e.WaitTimeBeforSpawnStarts,
                                                   e.TimeBetweenSpawns, e.Speed, e.PathGUID, e.HasSpawnFinished,
                                                   GetConnectedDependancyGUIDs(e), e.NodeRect, e.EnemyPoolRef));
            }
        }

        public static void Save(string DirectoryPath, string file)
        {
            FilePath = file;
            BuildEventsToSaveList();
            saveEvents(DirectoryPath);
        }

        public static void Load(string DirectoryPath, string file)
        {
            FilePath = file;
            _load(DirectoryPath);
        }
        public static void Load(string CurrentScene)
        {
            _load(CurrentScene);
        }

        public static void LoadEventsFromString(string xml)
        {
            EventsToSave = Serialize.SerializeFromXML<List<SaveEventData>>(xml);
            ReconstructEvents();
        }

        private static void _load(string DirectoryPath)
        {
            loadEvents(DirectoryPath);
            ReconstructEvents();
        }

        private static void ReconstructEvents()
        {
        EventNode.EventList = new List<EventNode>();
            //UnityEngine.Debug.Log("EventsToSave.count: " + EventsToSave.Count);
            foreach (SaveEventData data in EventsToSave)
            {
                EventNode newevent = new EventNode(data.myRect.MakeUnityRect(), data.NodeName, data.GUID,
                                                   data.NodeName, data.NumberOfEnimeys, data.WaitTimeBeforSpawnStarts,
                                                   data.TimeBetweenSpawns,
                                                   data.Speed, data.PathGUID, data.HasSpawned, data.EnemyPoolName);
                EventNode.AddToEventList(newevent);

                EventAndItsConnectonsDic.Add(newevent, data.GUIDDependancyArray);
            }

            //rebuild dependant list
            foreach (KeyValuePair<EventNode, string[]> keyValuePair in EventAndItsConnectonsDic)
            {
                //UnityEngine.Debug.Log("keyValuePair.Value" + keyValuePair.Value.Count());
                foreach (string dependancyGUID in keyValuePair.Value)
                {
                    //UnityEngine.Debug.Log("dependant guid" + dependantGUID);
                    //foreach (EventNode eventNode in EventNode.EventList)
                    //{
                    //    UnityEngine.Debug.Log("event node GUID" + eventNode.GUID);
                    //}
                    keyValuePair.Key.DependancyList.Add(EventNode.EventList.Find(e => e.GUID == dependancyGUID));
                    //EventNode.EventList.Find(e => e.GUID == dependancyGUID).DependancyList.Add(keyValuePair.Key);
                }
                //UnityEngine.Debug.Log("dependants list " + keyValuePair.Key.DependancyList.Count());

            }
        }



        //private static string DirectoryPath = "";
        private static string FilePath = "PacingEventData.xml";
        private static void loadEvents(string DirectoryPath)
        {
            //EventsToSave = Serialize.DeSerializeObject<List<SaveEventData>>(DirectoryPath + "/" + FilePath);
            //string xml = Serialize.DeSerializeObject<string>(DirectoryPath + "/" + FilePath);
            string xml =""; 
#if UNITY_WEBPLAYER
            string[] allLines = File.ReadAllLines(DirectoryPath + "/" + FilePath);
            foreach (string line in allLines)
            {
                xml += line;
            }
            
#else
            xml = File.ReadAllText(DirectoryPath + "/" + FilePath);
#endif
            //UnityEngine.Debug.Log("xml from file: "+ xml);
            EventsToSave = Serialize.SerializeFromXML<List<SaveEventData>>(xml);

            //UnityEngine.Debug.Log("EventsToSave.count: " + EventsToSave.Count);
        }

        private static void saveEvents(string DirectoryPath)
        {
            if (EventNode.EventList == null || EventsToSave.Count == 0) return; //no point in saving nothing
            try
            {
                // Debug.LogError("EnemeyPacing " + EnemeyPacing.ToString());
                //UnityEngine.Debug.Log("EventsToSave: " + EventsToSave);
                //UnityEngine.Debug.Log("DirectoryPath" + DirectoryPath);
                //UnityEngine.Debug.Log("FilePath" + FilePath);
                //Serialize.SerializeObject(EventsToSave, DirectoryPath, FilePath, true);
                Serialize.SerializeObject(Serialize.SerializeToXML(EventsToSave), DirectoryPath, FilePath, true);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
        }



        [Serializable]
        public class SaveEventData
        {
            public string GUID;
            public string NodeName = "";
            public int NumberOfEnimeys = 1;
            public float WaitTimeBeforSpawnStarts = 0;
            public float TimeBetweenSpawns = 0;
            public float Speed;
            //public S_Path Path;
            public string PathGUID;
            public bool HasSpawned = false;
            public string[] GUIDDependancyArray;
             [SerializeField]
            public NTRect myRect;

            public string EnemyPoolName;


            public SaveEventData()
            {}
            public SaveEventData(string guid, string nodeName, int numberOfEnimeys,
                                 float waitTimeBeforSpawnStarts, float timeBetweenSpawns,
                                 float speed, string pathGUID, bool hasSpawned, string[] guidDependancyArray,
                                 Rect sizePosRect, S_EnemyPool ep)
            {
                GUID = guid;
                NodeName = nodeName;
                NumberOfEnimeys = numberOfEnimeys;
                WaitTimeBeforSpawnStarts = waitTimeBeforSpawnStarts;
                TimeBetweenSpawns = timeBetweenSpawns;
                Speed = speed;
                PathGUID = pathGUID;
                HasSpawned = hasSpawned;
                GUIDDependancyArray = guidDependancyArray;

                myRect = new NTRect(sizePosRect);

                EnemyPoolName = ep.gameObject.name;
            }



            [Serializable]
            public class NTRect
            {
                [SerializeField]
                public float left;
                [SerializeField]
                public float top;
                [SerializeField]
                public float width;
                [SerializeField]
                public float height;

                public NTRect()
                {
                }

                public NTRect(Rect input)
                {
                    left = input.xMin;
                    top = input.yMin;
                    width = input.width;
                    height = input.height;
                }

                public Rect MakeUnityRect()
                {
                    return new Rect(left, top, width, height);
                }
            }
        }
    }
}
