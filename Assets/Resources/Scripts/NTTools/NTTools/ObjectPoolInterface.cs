using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTTools
{
	public interface IObjectPool
	{
        bool IsActive
        {
            get;
            set;
        }
	}
}
