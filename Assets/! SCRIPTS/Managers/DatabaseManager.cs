using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Manager
{
    [DefaultExecutionOrder(-25)]
    public class DatabaseManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<DatabaseTable> _tables;
        #endregion

        #region FIELDS PRIVATE
        private static DatabaseManager _instance;
        #endregion

        #region PROPERTIES
        public static DatabaseManager Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        #region METHODS PUBLIC
        public AbstractTable<T> GetTable<T>(string name) where T : ATableData
        {
            var tables = _tables.OfType<AbstractTable<T>>().ToList();
            if (tables.Count == 0)
            {
                Debug.LogError($"DatabaseManager: table with data type {typeof(T).Name} not found!");
                Debug.Break();
            }

            return tables.FirstOrDefault(e => e.name == name);
        }
        #endregion
    }
}