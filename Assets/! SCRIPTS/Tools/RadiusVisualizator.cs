using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(LineRenderer))]
    public class RadiusVisualizator : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(6, 360)] private int _precision;
        #endregion

        #region FIELDS PRIVATE
        private LineRenderer _lineRenderer;
        private float _radius = 0;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (_radius == 0) return;
            SetPointsOnLineRenderer();
        }
        #endregion

        #region METHODS PRIVATE
        private void SetPointsOnLineRenderer()
        {
            var points = new Vector3[_precision];
            for (int i = 0; i < _precision; i++)
            {
                var angle = (360f / _precision) * i;
                var point = GetPointOnCircleByAngle(transform.position, angle, _radius);
                points[i] = point;
            }

            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
        }

        private Vector3 GetPointOnCircleByAngle(Vector3 center, float angle, float radius)
        {
            var x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + center.x;
            var z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + center.z;
            var y = center.y;
            return new Vector3(x, y, z);
        }
        #endregion

        #region METHODS PUBLIC
        public void SetRadius(float radius)
        {
            _radius = radius;
        }
        #endregion
    }
}
