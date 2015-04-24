using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal class SubQueryCondition : Condition
    {
        public QueryConditionRelationType ConditionRelationType { get; set; }

        public string FieldName { get; set; }

        public QueryConditionOperatorType OperatorType { get; set; }

        public string SubQuerySQLTemplate { get; set; }

        public List<Condition> SubQueryConditions { get; set; }

        internal override void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            base.BuildQueryString(queryStringBuilder, buildContext);
            queryStringBuilder.Append(string.Format(" {0} {1} {2} ({3} {4})", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.ConditionRelationType).ToString(), (object)this.FieldName, (object)this.GetOperatorString(this.OperatorType), (object)this.SubQuerySQLTemplate, (object)ConditionConstructor.BuildQuerySqlConditionString(this.SubQueryConditions, new ConditionCollectionContext(buildContext.DataCommand)
            {
                AddedParameterNames = buildContext.AddedParameterNames
            })));
            buildContext.IsFirstCondition = false;
        }
    }
}
