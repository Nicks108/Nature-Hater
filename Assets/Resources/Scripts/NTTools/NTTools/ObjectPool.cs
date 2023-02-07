using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTTools
{
	public class ObjectPool<T> where T : IObjectPool
	{
        //private List<T> ObjectPool = new List<T>();
        //private T[] Array;
        private List<T> Pool;
        //private List<T> ActivePool;

        public new string ToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.Append(Pool.ToString());
            SB.Append("/n");
            SB.Append("Count: " + Count);
            SB.Append("/n");
            SB.Append("Inactive count: " + CountInactive);

            return SB.ToString();
        }

        public ObjectPool()
        {
            //Array = new T[size + 1];
            Pool = new List<T>();
        }

        public int Count
        {
            get { return Pool.Count; }
        }

        public int CountInactive
        {
            get 
            {
                int Inactive = 0;
                foreach (T value in Pool)
                {
                    //ObjectPoolInterface temp = value;
                    if (!value.IsActive)
                        Inactive++;
                }
                return Inactive;
            }
        }
        public int CountActive
        {
            get 
            {
                return Count - CountInactive;
            }
        }

        public T GetItem(int index)
        {
            if (index > Pool.Count)
                throw new Exception("NTTools: Index out of bounds for pool");

            return Pool[index];
        }

        public void SetItem(int index, T value)
        {
            if (index > Pool.Count)
                throw new Exception("NTTools: Index out of bounds for pool");
            Pool[index] = value;
        }

        public void push(T value)
        {
            Pool.Add(value);
        }
        public T pop()
        {
            T value = Pool[Pool.Count-1];
            Pool.Remove(value);
            return value;
        }

        public int GetIndexOfInactiveObject()
        {
            int returnIndex = -1;
            for (int i = 0; i < Pool.Count; i++)
            {
                if (!((IObjectPool)Pool[i]).IsActive)
                {
                    returnIndex = i;
                    break;
                }
            }

            return returnIndex;
        }
	}
}
