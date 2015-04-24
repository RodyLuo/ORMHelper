using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class CaseInsensitiveStringEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}
