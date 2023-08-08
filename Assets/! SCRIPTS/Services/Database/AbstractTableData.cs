using UnityEngine;

namespace Services.Database
{
    public abstract class AbstractTableData
    {
        public abstract T Copy<T>() where T : AbstractTableData;
    }
}
