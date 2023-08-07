using UnityEngine;

namespace Utility
{
    public class DrawGizmos : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private bool _on;
        [SerializeField] private Color _color;
        [SerializeField] private float _radius;
        #endregion

        #region UNITY CALLBACKS
        private void OnDrawGizmos()
        {
            if (!_on) return;

            var tempColor = Gizmos.color;

            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);

            Gizmos.color = tempColor;
        }
        #endregion
    }
}
