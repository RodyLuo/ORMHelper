using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.Entity
{
    public class QueryConditionEntity<T> where T : class, new()
    {
        public T Condition { get; set; }
        public PagingInfoEntity PagingInfo { get; set; }
    }
}
