using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Database
{
    [CreateAssetMenu(fileName = "NewPlayerCharacteristicUpgrade", menuName = "Tables/PlayerCharacteristicUpgrade", order = 0)]
    public class PlayerCharacteristicUpgrade : AbstractTable<PlayerUpgradeData>
    {
        #region METHODS PUBLIC
        public override void UpdateTableData(string[] rows)
        {
            var tableData = new List<PlayerUpgradeData>();
            for (int i = 1; i < rows.Length; i++)
            {
                var cells = rows[i].Split("\t");
                var data = new PlayerUpgradeData()
                {
                    value = ConvertStringToFloat(cells[0]),
                    cost = uint.Parse(cells[1])
                };

                tableData.Add(data);
            }

            _data = tableData;

            Debug.Log($"Database: {this.name} table, successful updated!");
        }
        #endregion
    }


}