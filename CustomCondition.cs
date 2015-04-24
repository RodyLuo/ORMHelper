using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    internal class CustomCondition : Condition
    {
        public QueryConditionRelationType ConditionRelationType { get; set; }

        public string CustomQueryString { get; set; }

        internal override void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            base.BuildQueryString(queryStringBuilder, buildContext);
            queryStringBuilder.Append(string.Format(" {0} {1}", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.ConditionRelationType).ToString(), (object)this.CustomQueryString));
            buildContext.IsFirstCondition = false;
        }
    }
}
