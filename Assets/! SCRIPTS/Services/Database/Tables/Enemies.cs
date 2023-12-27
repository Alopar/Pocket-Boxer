using UnityEngine;

namespace Services.Database
{
    [CreateAssetMenu(fileName = "NewEnemies", menuName = "Database/EnemiesTable", order = 1)]
    public class Enemies : AbstractTable<EnemyData> {}
}
