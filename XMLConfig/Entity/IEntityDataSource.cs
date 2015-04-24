using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity
{
    internal interface IEntityDataSource : IEnumerable<string>, IEnumerable, IDisposable
    {
        object this[string columnName] { get; }

        object this[int iIndex] { get; }

        bool ContainsColumn(string columnName);
    }
}
