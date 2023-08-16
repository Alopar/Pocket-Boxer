using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Database
{
    public abstract class AbstractTable<T> : DatabaseTable where T : AbstractTableData, new()
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected List<T> _datas;
        #endregion

        #region METHODS PUBLIC
        public override void UpdateTableData(string[] rows)
        {
            var type = typeof(T);
            var flags = BindingFlags.DeclaredOnly
                | BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic;
            var fields = type.GetFields(flags);

            var datas = new List<T>();
            var fieldParams = GetFieldParameters(rows[0]);
            for (int i = 1; i < rows.Length; i++)
            {
                var data = new T();
                var cells = rows[i].Split("\t");
                for (int j = 0; j < cells.Length; j++)
                {
                    var value = ParseValueByTypeName(fieldParams[j].type, cells[j]);
                    var field = fields.FirstOrDefault(e => e.Name == fieldParams[j].name);
                    if(field is null) continue;

                    field.SetValue(data, value);
                }

                datas.Add(data);
            }

            _datas = datas;
            Debug.Log($"Database: {this.name} table, successful updated!");
        }

        public T GetDataByIndex(uint value)
        {
            var index = (int)Mathf.Clamp(value, 0, _datas.Count - 1);
            return (T)_datas[index].Copy();
        }

        public int GetCount()
        {
            return _datas.Count;
        }
        #endregion

        #region METHODS PRIVATE
        private (string name, string type)[] GetFieldParameters(string headersRow)
        {
            var headers = headersRow.Split("\t");
            var result = new (string name, string type)[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                var data = headers[i].Split("#");
                result[i] = ($"_{data[0]}", data[1]);
            }

            return result;
        }

        private object ParseValueByTypeName(string typeName, string value)
        {
            object result = null;
            switch (typeName)
            {
                case "string":
                    result = value;
                    break;
                case "int":
                    result = int.Parse(value);
                    break;
                case "uint":
                    result = uint.Parse(value);
                    break;
                case "float":
                    result = ConvertStringToFloat(value);
                    break;
            }

            return result;
        }

        private float ConvertStringToFloat(string value)
        {
            //float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
            float.TryParse(value, out float result);
            return result;
        }
        #endregion
    }
}
