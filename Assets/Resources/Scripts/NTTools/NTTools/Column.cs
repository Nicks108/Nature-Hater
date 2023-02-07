using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTTools
{
    [Serializable]
    public class Column
    {
        public List<S_ImageButton> Row;
        public static int GetLargestRowCount(List<Column> Columns )
        {
            int biggest = 0;
            foreach (Column column in Columns)
            {
                if(column.Row.Count > biggest) biggest = column.Row.Count;
            }
            return biggest;
        }
    }
}
