using UnityEngine;

namespace Services.PointerSystem
{
    public class Target
    {
        #region FIELDS PRIVATE
        private readonly Transform _transform;
        private readonly PointerType _pointerType;
        #endregion

        #region PROPERTIES
        public Transform Transform => _transform;
        public PointerType PointerType => _pointerType;
        #endregion

        #region CONSTRUCTORS
        public Target(Transform transform, PointerType pointerType)
        {
            _transform = transform;
            _pointerType = pointerType;
        }
        #endregion
    }
}
