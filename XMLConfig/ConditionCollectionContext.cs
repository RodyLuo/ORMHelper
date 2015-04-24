using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal class ConditionCollectionContext
    {
        public bool IsFirstCondition { get; set; }

        public Stack<bool> FirstGroupConditionFlags { get; set; }

        public CustomDataCommand DataCommand { get; set; }

        public List<string> AddedParameterNames { get; set; }

        public ConditionCollectionContext(CustomDataCommand contextDataCommand)
        {
            this.IsFirstCondition = true;
            this.FirstGroupConditionFlags = new Stack<bool>();
            this.AddedParameterNames = new List<string>();
            this.DataCommand = contextDataCommand;
        }
    }
}
