using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Database
{
    public abstract class AbstractTable<T> : DatabaseTable where T : AbstractTableData
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected List<T> _data;
        #endregion

        #region METHODS PUBLIC
        public T GetDataByIndex(uint value)
        {
            var index = (int)Mathf.Clamp(value, 0, _data.Count - 1);
            return _data[index].Copy<T>();
        }

        public int GetCount()
        {
            return _data.Count;
        }
        #endregion
    }
}
