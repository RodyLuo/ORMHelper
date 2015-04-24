using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal class RightBraketCondition : Condition
    {
        internal override void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            base.BuildQueryString(queryStringBuilder, buildContext);
            if (buildContext.FirstGroupConditionFlags.Count <= 0)
                return;
            bool flag = buildContext.FirstGroupConditionFlags.Pop();
            string str = ((object)queryStringBuilder).ToString().Trim();
            if (str.Substring(str.Length - 1, 1) == "(")
            {
                if (flag)
                {
                    queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
                }
                else
                {
                    queryStringBuilder.Remove(queryStringBuilder.Length - 5, 5);
                    buildContext.IsFirstCondition = false;
                }
            }
            else
            {
                queryStringBuilder.Append(" )");
                buildContext.IsFirstCondition = false;
            }
        }
    }
}
