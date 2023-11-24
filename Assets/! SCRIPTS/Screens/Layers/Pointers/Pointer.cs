using UnityEngine;
using PointerType = Services.PointerSystem.PointerType;

namespace Gameplay
{
    public class Pointer : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private RectTransform _pivot;
        [SerializeField] private RectTransform _Icon;

        [Space(10)]
        [SerializeField] private PointerType _type;
        #endregion

        #region PROPERTIES
        public RectTransform Pivot => _pivot;
        public RectTransform Icon => _Icon;
        public PointerType Type => _type;
        #endregion
    }
}
