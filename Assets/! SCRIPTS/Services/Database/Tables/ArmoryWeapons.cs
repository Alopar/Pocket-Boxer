using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Database
{
    [CreateAssetMenu(fileName = "NewArmoryWeapons", menuName = "Tables/ArmoryWeapons", order = 0)]
    public class ArmoryWeapons : AbstractTable<ArmoryWeaponsData>
    {
        #region METHODS PUBLIC
        public override void UpdateTableData(string[] rows)
        {
            var tableData = new List<ArmoryWeaponsData>();
            for (int i = 1; i < rows.Length; i++)
            {
                var cells = rows[i].Split("\t");
                var data = new ArmoryWeaponsData()
                {
                    id = int.Parse(cells[0]),
                    name = cells[1],
                    cost = uint.Parse(cells[2])
                };

                tableData.Add(data);
            }

            _data = tableData;

            Debug.Log($"Database: {this.name} table, successful updated!");
        }
        #endregion
    }
}