using System.Collections.Generic;
using UnityEngine;
using Tayx.Graphy;

namespace Services.ScreenSystem
{
    [CreateAssetMenu(fileName = "NewScreenContainer", menuName = "Containers/ScreenContainer", order = 0)]
    public class ScreenContainer : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<AbstractScreenController> _screenPrefabs;

        [Space(10)]
        [SerializeField] private GraphyManager _monitoringPrefab;
        #endregion

        #region PROPERTIES
        public List<AbstractScreenController> ScreenPrefabs => _screenPrefabs;
        public GraphyManager MonitoringPrefab => _monitoringPrefab;
        #endregion
    }
}
