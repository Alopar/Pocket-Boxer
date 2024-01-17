using System.Collections.Generic;
using UnityEngine;
using Tayx.Graphy;
using IngameDebugConsole;

namespace Services.ScreenSystem
{
    [CreateAssetMenu(fileName = "NewScreenContainer", menuName = "Containers/ScreenContainer", order = 0)]
    public class ScreenContainer : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<AbstractScreen> _screenPrefabs;

        [Space(10)]
        [SerializeField] private GraphyManager _monitoringPrefab;
        [SerializeField] private DebugLogManager _debugConsolePrefab;
        #endregion

        #region PROPERTIES
        public List<AbstractScreen> ScreenPrefabs => _screenPrefabs;
        public GraphyManager MonitoringPrefab => _monitoringPrefab;
        public DebugLogManager DebugConsolePrefab => _debugConsolePrefab;
        #endregion
    }
}
