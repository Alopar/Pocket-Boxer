using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Gameplay
{
    public abstract class DatabaseTable : ScriptableObject 
    {
        #region FIELDS INSPECTOR
        [SerializeField] private bool _isWebUpdatable;

        [Tooltip("URL for download data from Google Sheets")]
        [ShowIf("_isWebUpdatable"), SerializeField] private string _url;
        #endregion

        #region PROPERTIES
        public bool IsUpdatable => _isWebUpdatable;
        public string URL => _url;
        #endregion

        #region METHODS PRIVATE
        protected float ConvertStringToFloat(string value)
        {
            //float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
            float.TryParse(value, out float result);
            return result;
        }
        #endregion

        #region METHODS PUBLIC
        public abstract void UpdateTableData(string[] rows);
        #endregion
    }

    public abstract class AbstractTable<T> : DatabaseTable where T : ATableData
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

    public abstract class ATableData
    {
        public abstract T Copy<T>() where T : ATableData;
    }
}
