using NaughtyAttributes;
using UnityEngine;

namespace Services.Database
{
    public abstract class DatabaseTable : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private bool _isUsed;
        [SerializeField] private bool _isWebUpdatable;
        [Tooltip("URL for download data from Google Sheets")]
        [ShowIf("_isWebUpdatable"), SerializeField] private string _url;
        #endregion

        #region PROPERTIES
        public bool IsUsed => _isUsed;

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
}
