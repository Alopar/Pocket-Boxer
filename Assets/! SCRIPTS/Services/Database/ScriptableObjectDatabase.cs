using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Database
{
    public class ScriptableObjectDatabase : IDatabaseService
    {
        #region FIELDS PRIVATE
        private readonly List<DatabaseTable> _tables;
        #endregion

        #region CONSTRUCTORS
        public ScriptableObjectDatabase()
        {
            _tables = FindDatabaseTables();
        }
        #endregion

        #region METHODS PRIVATE
        private List<DatabaseTable> FindDatabaseTables()
        {
            var result = new List<DatabaseTable>();
            var tables = Resources.LoadAll<DatabaseTable>("").ToList();
            foreach (var table in tables)
            {
                if(!table.IsUsed) continue;
                result.Add(table);
            }

            return result;
        }
        #endregion

        #region METHODS PUBLIC
        public AbstractTable<T> GetTable<T>(string name) where T : AbstractTableData
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