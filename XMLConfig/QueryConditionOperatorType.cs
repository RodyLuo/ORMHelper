using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    /// <summary>
    /// 条件枚举类型
    /// </summary>
    public enum QueryConditionOperatorType
    {
        Equal,
        NotEqual,
        MoreThan,
        LessThan,
        MoreThanOrEqual,
        LessThanOrEqual,
        Like,
        IsNull,
        IsNotNull,
        In,
        NotIn,
        Exist,
        NotExist,
        LeftLike,
        RightLike,
    }
}
