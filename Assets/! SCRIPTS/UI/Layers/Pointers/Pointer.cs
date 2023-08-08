using UnityEngine;

namespace Gameplay
{
    public class Pointer : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private RectTransform _pivot;
        [SerializeField] private RectTransform _Icon;
        #endregion

        #region PROPERTIES
        public RectTransform Pivot => _pivot;
        public RectTransform Icon => _Icon;
        #endregion
    }
}