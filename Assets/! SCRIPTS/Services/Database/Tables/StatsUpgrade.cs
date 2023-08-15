using UnityEngine;

namespace Services.Database
{
    [CreateAssetMenu(fileName = "NewStatsUpgrade", menuName = "Database/StatsUpgradeTable", order = 0)]
    public class StatsUpgrade : AbstractTable<StatsUpgradeData> {}
}
