using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conveyor
{
    internal class CCarrier
    {
        public UInt64 id;
        public UInt16 source;
        public UInt16 dest;
        public USE use;

        public enum USE
        {
            USE_NONE,
            USE_TAKEOUT,
            USE_STACK
        }

        public void Clear()
        {
            id = 0;
            source = 0;
            dest = 0;
            use = USE.USE_NONE;
        }
    }
}
