using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal class LeftBracketCondition : Condition
    {
        public QueryConditionRelationType GroupConditionRelationType { get; set; }

        internal override void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            base.BuildQueryString(queryStringBuilder, buildContext);
            queryStringBuilder.Append(string.Format(" {0} (", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.GroupConditionRelationType).ToString()));
            buildContext.FirstGroupConditionFlags.Push(buildContext.IsFirstCondition);
            buildContext.IsFirstCondition = true;
        }
    }
}
