using System;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

namespace Gameplay
{
    public class CargoComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _cargoPoint;

        [Space(10)]
        [SerializeField, Range(0, 1)] private float _layerOffset;
        [SerializeField] private List<Vector3> _cargoOffsets;

        [Header("DEBUG SETTINGS:")]
        [SerializeField] private bool _gizmoEnabled;
        [SerializeField] private Color _gizmoColor;
        [SerializeField, Range(0, 1)] private float _gizmoRadius;
        [SerializeField, Range(0, 300)] private int _gizmoNumbers;
        #endregion

        #region FIELDS PRIVATE
        private const int MAX_CARGO = 360;
        private MagnetComponent _magnetComponent;

        private List<Transform> _cargoPoints = new();
        
        private int _capacity = 36;
        private int _occupied = 0;
        private Stack<Transform> _cargo = new();
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnDrawGizmos()
        {
            if (!_gizmoEnabled) return;
            DrawCargoPoints();
        }

        private void FixedUpdate()
        {
            if (_occupied >= _capacity) return;
            CollectCargo();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            ResolveDependency();
            CreateCargoPoints();
        }

        private void ResolveDependency()
        {
            _magnetComponent = GetComponent<MagnetComponent>();
        }

        private void ChangeCapacityByValue(int value)
        {
            _capacity += value;
        }

        private void ChangeOccupiedByValue(int value)
        {
            _occupied += value;
        }

        private void DrawCargoPoints()
        {
            var gizmoCounter = 0;
            var numberLayers = _gizmoNumbers / _cargoOffsets.Count;
            for (int i = 0; i <= numberLayers; i++)
            {
                foreach (var offset in _cargoOffsets)
                {
                    if (gizmoCounter == _gizmoNumbers) return;

                    var gizmoPosition = _cargoPoint.position + offset;
                    gizmoPosition.y += i * _layerOffset;

                    DrawGizmo(gizmoPosition);

                    gizmoCounter++;
                }
            }
        }

        private void DrawGizmo(Vector3 position)
        {
            var tempColor = Gizmos.color;
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(position, _gizmoRadius);
            Gizmos.color = tempColor;
        }

        private void CreateCargoPoints()
        {
            _cargoPoints.Clear();

            var cargoCounter = 0;
            var numberLayers = MAX_CARGO / _cargoOffsets.Count;
            for (int i = 0; i <= numberLayers; i++)
            {
                foreach (var offset in _cargoOffsets)
                {
                    if (cargoCounter == MAX_CARGO) return;

                    var cargoPosition = _cargoPoint.position + offset;
                    cargoPosition.y += i * _layerOffset;

                    var cargoPoint = CreatePoint(cargoPosition, cargoCounter);
                    _cargoPoints.Add(cargoPoint);

                    cargoCounter++;
                }
            }
        }

        private Transform CreatePoint(Vector3 position, int number)
        {
            var cargoPoint = new GameObject($"CargoPoint.{number:00}");
            cargoPoint.transform.position = position;
            cargoPoint.transform.parent = _cargoPoint.transform;

            return cargoPoint.transform;
        }

        private void CollectCargo()
        {
            var callbacks = new Queue<Action<Transform>>();
            var numberEmptyPoints = _capacity - _occupied;
            for (int i = 0; i < numberEmptyPoints; i++)
            {
                var pointIndex = _occupied + i;
                var point = _cargoPoints[pointIndex];

                Action<Transform> callback = (Transform target) => {
                    target.transform.position = point.position;
                    target.transform.parent = point;
                    _cargo.Push(target);

                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                };

                callbacks.Enqueue(callback);
            }

            _magnetComponent.MagnetObjectsByCallbacks(callbacks);
            ChangeOccupiedByValue(numberEmptyPoints - callbacks.Count);
        }
        #endregion

        #region METHODS PUBLIC
        public bool TryGetCargo(out Transform cargo)
        {
            cargo = null;
            if(_cargo.Count == 0) return false;

            cargo = _cargo.Pop();
            cargo.transform.parent = null;
            ChangeOccupiedByValue(-1);

            return true;
        }

        public void SetCapacity(int value)
        {
            _capacity = value;
        }
        #endregion
    }
}
