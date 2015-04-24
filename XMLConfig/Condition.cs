using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal abstract class Condition
    {
        internal virtual void BuildQueryString(StringBuilder queryStringBuilder, ConditionCollectionContext buildContext)
        {
            if (queryStringBuilder == null)
                throw new ArgumentException("Input query string builder can not be null.");
            if (buildContext == null)
                throw new ArgumentException("Input build context can not be null.");
        }

        protected string GetOperatorString(QueryConditionOperatorType operatorType)
        {
            switch (operatorType)
            {
                case QueryConditionOperatorType.Equal:
                    return "=";
                case QueryConditionOperatorType.NotEqual:
                    return "<>";
                case QueryConditionOperatorType.MoreThan:
                    return ">";
                case QueryConditionOperatorType.LessThan:
                    return "<";
                case QueryConditionOperatorType.MoreThanOrEqual:
                    return ">=";
                case QueryConditionOperatorType.LessThanOrEqual:
                    return "<=";
                case QueryConditionOperatorType.Like:
                case QueryConditionOperatorType.LeftLike:
                case QueryConditionOperatorType.RightLike:
                    return "LIKE";
                case QueryConditionOperatorType.IsNull:
                    return "IS NULL";
                case QueryConditionOperatorType.IsNotNull:
                    return "IS NOT NULL";
                case QueryConditionOperatorType.In:
                    return "IN";
                case QueryConditionOperatorType.NotIn:
                    return "NOT IN";
                case QueryConditionOperatorType.Exist:
                    return "EXISTS";
                case QueryConditionOperatorType.NotExist:
                    return "NOT EXISTS";
                default:
                    return string.Empty;
            }
        }

        protected object TryConvertToLikeString(object value, QueryConditionOperatorType type)
        {
            if (value != null && value.GetType().Equals(typeof(string)) && !ConditionConstructor.IsStringNullOrEmpty(value.ToString()))
            {
                if (type == QueryConditionOperatorType.Like)
                    return (object)("%" + value.ToString() + "%");
                if (type == QueryConditionOperatorType.LeftLike)
                    return (object)(value.ToString() + "%");
                if (type == QueryConditionOperatorType.RightLike)
                    return (object)("%" + value.ToString());
            }
            return value;
        }
    }
}
