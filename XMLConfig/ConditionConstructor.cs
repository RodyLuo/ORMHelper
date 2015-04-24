using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class ConditionConstructor
    {
        private List<Condition> m_conditions;

        internal List<Condition> Conditions
        {
            get
            {
                return this.m_conditions;
            }
        }

        public int ValidateConditionCount
        {
            get
            {
                if (this.m_conditions == null)
                    return 0;
                else
                    return this.m_conditions.Count;
            }
        }

        internal ConditionConstructor()
        {
            this.m_conditions = new List<Condition>();
        }

        internal ConditionConstructor(List<Condition> conditions)
        {
            this.m_conditions = conditions;
        }

        internal static bool IsStringNullOrEmpty(string value)
        {
            return value == null || value.Trim() == string.Empty;
        }

        internal static bool DefaultParameterValueValidationCheck(object parameterValue)
        {
            return parameterValue != null && (!parameterValue.GetType().Equals(typeof(string)) || !ConditionConstructor.IsStringNullOrEmpty(parameterValue.ToString()));
        }

        private void AddInOrNotInCondition(QueryConditionOperatorType operationType, QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<object> inValues)
        {
            if (operationType != QueryConditionOperatorType.In && operationType != QueryConditionOperatorType.NotIn)
                throw new ArgumentException("Operation Type must be 'In' or 'NotIn'.");
            this.AddCondition(conditionRelationType, fieldName, listValueDbType, (string)null, operationType, (object)inValues, (ParameterValueValidateCheckDelegate)(value =>
            {
                if (inValues != null)
                    return inValues.Count > 0;
                else
                    return false;
            }));
        }

        private void AddInOrNotInCondition<TListValueType>(QueryConditionOperatorType operationType, QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<TListValueType> inValues)
        {
            List<object> inValues1 = new List<object>();
            if (inValues != null)
            {
                foreach (TListValueType listValueType in inValues)
                    inValues1.Add((object)listValueType);
            }
            this.AddInOrNotInCondition(operationType, conditionRelationType, fieldName, listValueDbType, inValues1);
        }

        public void AddCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType parameterDbType, string parameterName, QueryConditionOperatorType conditionOperatorType, object parameterValue, ParameterValueValidateCheckDelegate parameterValueValidateCheckHandler)
        {
            if (parameterValueValidateCheckHandler == null)
                parameterValueValidateCheckHandler = new ParameterValueValidateCheckDelegate(ConditionConstructor.DefaultParameterValueValidationCheck);
            if (!parameterValueValidateCheckHandler(parameterValue))
                return;
            this.m_conditions.Add((Condition)new SqlCondition()
            {
                ConditionRelationType = conditionRelationType,
                ParameterDbType = parameterDbType,
                FieldName = fieldName,
                ParameterName = parameterName,
                OperatorType = conditionOperatorType,
                ParameterValue = parameterValue
            });
        }

        public void BeginGroupCondition(QueryConditionRelationType groupRelationType)
        {
            this.m_conditions.Add((Condition)new LeftBracketCondition()
            {
                GroupConditionRelationType = groupRelationType
            });
        }

        public void EndGroupCondition()
        {
            this.m_conditions.Add((Condition)new RightBraketCondition());
        }

        public void AddCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType parameterDbType, string parameterName, QueryConditionOperatorType conditionOperatorType, object parameterValue)
        {
            this.AddCondition(conditionRelationType, fieldName, parameterDbType, parameterName, conditionOperatorType, parameterValue, new ParameterValueValidateCheckDelegate(ConditionConstructor.DefaultParameterValueValidationCheck));
        }

        public void AddBetweenCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType parameterDbType, string parameterName, QueryConditionOperatorType leftConditionOperatorType, QueryConditionOperatorType rightConditionOperatorType, object leftParameterValue, object rightParameterValue, ParameterValueValidateCheckDelegate parameterValueValidateCheckHandler)
        {
            this.BeginGroupCondition(conditionRelationType);
            this.AddCondition(QueryConditionRelationType.AND, fieldName, parameterDbType, parameterName + "_Left", leftConditionOperatorType, leftParameterValue, parameterValueValidateCheckHandler);
            this.AddCondition(QueryConditionRelationType.AND, fieldName, parameterDbType, parameterName + "_Right", rightConditionOperatorType, rightParameterValue, parameterValueValidateCheckHandler);
            this.EndGroupCondition();
        }

        public void AddBetweenCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType parameterDbType, string parameterName, QueryConditionOperatorType leftConditionOperatorType, QueryConditionOperatorType rightConditionOperatorType, object leftParameterValue, object rightParameterValue)
        {
            this.AddBetweenCondition(conditionRelationType, fieldName, parameterDbType, parameterName, leftConditionOperatorType, rightConditionOperatorType, leftParameterValue, rightParameterValue, new ParameterValueValidateCheckDelegate(ConditionConstructor.DefaultParameterValueValidationCheck));
        }

        public void AddNullCheckCondition(QueryConditionRelationType conditionRelationType, string fieldName, QueryConditionOperatorType conditionOperatorType)
        {
            if (conditionOperatorType != QueryConditionOperatorType.IsNull && conditionOperatorType != QueryConditionOperatorType.IsNotNull)
                throw new ArgumentException("Parameter conditionOperatorType must be IsNull or IsNotNull in this method.");
            this.AddCondition(conditionRelationType, fieldName, DbType.Object, (string)null, conditionOperatorType, (object)null, (ParameterValueValidateCheckDelegate)(value => true));
        }

        public void AddInCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<object> inValues)
        {
            this.AddInOrNotInCondition(QueryConditionOperatorType.In, conditionRelationType, fieldName, listValueDbType, inValues);
        }

        public void AddNotInCondition(QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<object> inValues)
        {
            this.AddInOrNotInCondition(QueryConditionOperatorType.NotIn, conditionRelationType, fieldName, listValueDbType, inValues);
        }

        public void AddInCondition<TListValueType>(QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<TListValueType> inValues)
        {
            this.AddInOrNotInCondition<TListValueType>(QueryConditionOperatorType.In, conditionRelationType, fieldName, listValueDbType, inValues);
        }

        public void AddNotInCondition<TListValueType>(QueryConditionRelationType conditionRelationType, string fieldName, DbType listValueDbType, List<TListValueType> inValues)
        {
            this.AddInOrNotInCondition<TListValueType>(QueryConditionOperatorType.NotIn, conditionRelationType, fieldName, listValueDbType, inValues);
        }

        public ConditionConstructor AddSubQueryCondition(QueryConditionRelationType conditionRelationType, string filedName, QueryConditionOperatorType conditionOperatorType, string subQuerySQLTemplate)
        {
            if (ConditionConstructor.IsStringNullOrEmpty(subQuerySQLTemplate))
                return (ConditionConstructor)null;
            SubQueryCondition subQueryCondition = new SubQueryCondition()
            {
                ConditionRelationType = conditionRelationType,
                FieldName = filedName,
                OperatorType = conditionOperatorType,
                SubQuerySQLTemplate = subQuerySQLTemplate,
                SubQueryConditions = new List<Condition>()
            };
            this.m_conditions.Add((Condition)subQueryCondition);
            return new ConditionConstructor(subQueryCondition.SubQueryConditions);
        }

        public void AddCustomCondition(QueryConditionRelationType conditionRelationType, string customQueryString)
        {
            this.m_conditions.Add((Condition)new CustomCondition()
            {
                ConditionRelationType = conditionRelationType,
                CustomQueryString = customQueryString
            });
        }

        internal static string BuildQuerySqlConditionString(List<Condition> conditions, ConditionCollectionContext buildContext)
        {
            if (conditions == null || conditions.Count == 0)
                return string.Empty;
            StringBuilder queryStringBuilder = new StringBuilder();
            queryStringBuilder.Append("WHERE ");
            foreach (Condition condition in conditions)
                condition.BuildQueryString(queryStringBuilder, buildContext);
            string str = ((object)queryStringBuilder).ToString().Trim();
            if (!(str == "WHERE"))
                return str;
            else
                return string.Empty;
        }

        internal string BuildQuerySqlConditionString(ConditionCollectionContext buildContext)
        {
            return ConditionConstructor.BuildQuerySqlConditionString(this.m_conditions, buildContext);
        }
    }
}
