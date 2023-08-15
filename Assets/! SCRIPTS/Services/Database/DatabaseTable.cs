using UnityEngine;
using NaughtyAttributes;

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

        #region METHODS PUBLIC
        public abstract void UpdateTableData(string[] rows);
        #endregion
    }
}
