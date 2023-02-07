using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NTTools.Data_Structures
{
    [Serializable]
    public class NTEnamyPaceData
    {
        //pacing data
        public string NodeName ="";
        public int NumberOfEnimeys =1;
        public float WaitTimeBeforSpawnStarts = 0;
        public float TimeBetweenSpawns = 0;
        public bool isRepeat = false;

        //window Data

        public NTEnamyPaceData()
        {
        }
        public NTEnamyPaceData(string NodeName)
        {
            this.NodeName = NodeName;
        }
        public NTEnamyPaceData(string NodeName, int NumberOfEnimeys, float WaitTimeBeforSpawnStarts, float TimeBetweenSpawns, bool isRepeat)
        {
            this.NodeName = NodeName;
            this.NumberOfEnimeys = NumberOfEnimeys;
            this.WaitTimeBeforSpawnStarts = WaitTimeBeforSpawnStarts;
            this.TimeBetweenSpawns =TimeBetweenSpawns;
            this.isRepeat =isRepeat;
        }
        /// <summary>
        /// toString via class reflection
        /// </summary>
        /// <returns></returns>
        public override string ToString()
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
}
