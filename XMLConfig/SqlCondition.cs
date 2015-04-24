using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal class SqlCondition : Condition
    {
        public string FieldName { get; set; }

        public string ParameterName { get; set; }

        public object ParameterValue { get; set; }

        public DbType ParameterDbType { get; set; }

        public QueryConditionOperatorType OperatorType { get; set; }

        public QueryConditionRelationType ConditionRelationType { get; set; }

        private string BuildSqlConditionString(ConditionCollectionContext buildContext)
        {
            string str1 = string.Empty;
            string str2;
            if (this.OperatorType == QueryConditionOperatorType.IsNull || this.OperatorType == QueryConditionOperatorType.IsNotNull)
                str2 = string.Format(" {0} {1} {2}", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.ConditionRelationType).ToString(), (object)this.FieldName, (object)this.GetOperatorString(this.OperatorType));
            else if (this.OperatorType == QueryConditionOperatorType.In || this.OperatorType == QueryConditionOperatorType.NotIn)
            {
                List<object> list = this.ParameterValue as List<object>;
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < list.Count; ++index)
                {
                    string name = string.Format("@{0}_Values{1}", (object)this.FieldName.Replace(".", string.Empty), (object)index);
                    stringBuilder.AppendFormat("{0},", (object)name);
                    buildContext.DataCommand.AddInputParameter(name, this.ParameterDbType, list[index]);
                }
                string str3 = ((object)stringBuilder).ToString();
                string str4 = str3.Substring(0, str3.Length - 1);
                str2 = string.Format(" {0} {1} {2} ({3})", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.ConditionRelationType).ToString(), (object)this.FieldName, (object)this.GetOperatorString(this.OperatorType), (object)str4);
            }
            else
            {
                str2 = string.Format(" {0} {1} {2} {3}", buildContext.IsFirstCondition ? (object)string.Empty : (object)((object)this.ConditionRelationType).ToString(), (object)this.FieldName, (object)this.GetOperatorString(this.OperatorType), (object)this.ParameterName);
                if (!buildContext.AddedParameterNames.Contains(this.ParameterName))
                {
                    buildContext.DataCommand.AddInputParameter(this.ParameterName, this.ParameterDbType, this.OperatorType == QueryConditionOperatorType.Like || this.OperatorType == QueryConditionOperatorType.LeftLike || this.OperatorType == QueryConditionOperatorType.RightLike ? this.TryConvertToLikeString(this.ParameterValue, this.OperatorType) : this.ParameterValue);
                    buildContext.AddedParameterNames.Add(this.ParameterName);
                }
            }
            return str2;
        }

        internal override void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            base.BuildQueryString(queryStringBuilder, buildContext);
            queryStringBuilder.Append(this.BuildSqlConditionString(buildContext));
            buildContext.IsFirstCondition = false;
        }
    }
}
