using System;
using System.Collections.Generic;
using System.Data;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;
using Ctrip.Flight.OrderProcess.DataBaseHelper.Entity;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public class DynamicQuerySqlBuilder : IDisposable
    {
        private const int Default_PageSize = 1000;
        private ConditionConstructor m_conditionConstructor;
        private PagingInfoEntity m_pagingInfo;
        private CustomDataCommand m_dataCommand;
        private string m_querySqlTemplate;
        private string m_defaultOrderBy;

        public ConditionConstructor ConditionConstructor
        {
            get
            {
                return this.m_conditionConstructor;
            }
        }

        public DynamicQuerySqlBuilder(string querySqlTemplate, CustomDataCommand dataCommand, PagingInfoEntity pagingInfo, string defaultOrderBy)
        {
            this.m_pagingInfo = pagingInfo;
            this.m_dataCommand = dataCommand;
            this.m_querySqlTemplate = querySqlTemplate;
            this.m_defaultOrderBy = defaultOrderBy;
            this.m_conditionConstructor = new ConditionConstructor();
        }

        private string BuildOrderByString()
        {
            string str = this.m_defaultOrderBy;
            if (this.m_pagingInfo != null && !ConditionConstructor.IsStringNullOrEmpty(this.m_pagingInfo.SortField))
                str = this.m_pagingInfo.SortField;
            if (ConditionConstructor.IsStringNullOrEmpty(str))
                throw new ApplicationException("Daynamic query must have one OrderBy field at least.");
            else
                return str;
        }

        private void SetPagingInformation()
        {
            int num1 = 1000;
            int num2 = 0;
            if (this.m_pagingInfo != null)
            {
                if (this.m_pagingInfo.MaximumRows.HasValue)
                    num1 = this.m_pagingInfo.MaximumRows.Value;
                if (this.m_pagingInfo.StartRowIndex.HasValue)
                    num2 = this.m_pagingInfo.StartRowIndex.Value;
            }
            this.m_dataCommand.AddInputParameter("@EndNumber", DbType.Int32, (object)(num1 + num2));
            this.m_dataCommand.AddInputParameter("@StartNumber", DbType.Int32, (object)num2);
            this.m_dataCommand.AddOutParameter("@TotalCount", DbType.Int32, 4);
        }

        public string BuildQuerySql()
        {
            string str = this.m_querySqlTemplate.Replace("#StrWhere#", this.m_conditionConstructor.BuildQuerySqlConditionString(new ConditionCollectionContext(this.m_dataCommand))).Replace("#SortColumnName#", this.BuildOrderByString());
            this.SetPagingInformation();
            return str;
        }

        public string GetBuildQueryWhere(string suffix)
        {
            StringBuilder sb = new StringBuilder(100);
            if (this.ConditionConstructor.Conditions != null && this.ConditionConstructor.Conditions.Count > 0)
            {

                for (int i = 0; i < this.ConditionConstructor.Conditions.Count; i++)
                {
                    var codition = this.ConditionConstructor.Conditions[i] as CustomCondition;
                    if (codition != null)
                    {
                        sb.Append(codition.ConditionRelationType.ToString());
                        sb.Append(codition.CustomQueryString);
                    }
                    else
                    {
                        var sqlCondition = this.ConditionConstructor.Conditions[i] as SqlCondition;
                        if (sqlCondition != null)
                        {
                            sb.Append(sqlCondition.FieldName);
                            sb.Append(sqlCondition.ConditionRelationType.ToString());
                            sb.Append(sqlCondition.OperatorType.ToString());
                            sb.Append(sqlCondition.ParameterValue.ToString());
                        }
                    }
                }

            }
            sb.Append(suffix);
            return sb.ToString();
        }

        public void Dispose()
        {
            try
            {
                if (this.m_pagingInfo == null)
                    return;
                object parameterValue = this.m_dataCommand.GetParameterValue("@TotalCount");
                if (parameterValue == null || parameterValue == DBNull.Value)
                    return;
                this.m_pagingInfo.TotalCount = Convert.ToInt32(parameterValue);
            }
            catch
            {
            }
        }
    }
}
