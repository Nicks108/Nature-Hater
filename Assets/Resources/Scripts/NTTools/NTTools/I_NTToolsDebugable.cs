using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTTools.Debug
{
    interface I_NTToolsDebugable
    {
        bool _isDebugging
        {
            get;
            set;
        }
        bool _isBreaking
        {
            get;
            set;
        }
    }
}
