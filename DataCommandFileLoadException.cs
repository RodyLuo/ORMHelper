using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public class DataCommandFileLoadException : Exception
    {
        public DataCommandFileLoadException(string fileName)
            : base("DataCommand file " + fileName + " not found or is invalid.")
        {
        }
    }
}
