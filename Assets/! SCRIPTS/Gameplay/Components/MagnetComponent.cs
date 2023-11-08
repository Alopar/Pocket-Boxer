using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Tools;
using Utility.GameSettings;

namespace Gameplay
{
    public class MagnetComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _magnetPoint;
        [SerializeField] private float _magnetDistance;
        [SerializeField] private float _magnetSpeed;

        [Space(10)]
        [SerializeField, Range(0, 10)] private int _pathPrecision;
        [SerializeField] private AnimationCurve _pathCurve;

        [Space(10)]
        [SerializeField] private LayerMask _layerMask;

        [Header("VISUALISATION SETTINGS:")]
        [SerializeField] private RadiusVisualizator _magnetVisualisator;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            UpdateMarkers();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _magnetVisualisator.gameObject.SetActive(GameSettings.Data.ShowDebugMarkers);
        }

        private List<Transform> FindTargets(int maxTargets)
        {
            var counter = 0;
            var targets = new List<Transform>();
            var magnetables = Physics.OverlapSphere(transform.position, _magnetDistance, _layerMask);
            foreach (var magnetable in magnetables)
            {
                if (counter == maxTargets) break;
                if (magnetable.TryGetComponent<IMagnetable>(out var component))
                {
                    if (component.TryAttract())
                    {
                        targets.Add(magnetable.transform);
                    }
                }

                counter++;
            }

            return targets;
        }

        private void SetupAttractorBeam(Transform target, Action<Transform> callback)
        {
            StartCoroutine(AttractorBeam(target, callback));
        }

        private void UpdateMarkers()
        {
            _magnetVisualisator.SetRadius(_magnetDistance);
        }

        private SplineContainer CreatePath()
        {
            var gameObject = new GameObject("AttactorBeamPath");
            var path = gameObject.AddComponent<SplineContainer>();
            return path;
        }

        private void UpdateSpline(Spline spline, Vector3 startPoint, Vector3 endPoint, int precision, AnimationCurve curve)
        {
            spline.Clear();

            var direction = endPoint - startPoint;
            for (int i = 0; i <= precision; i++)
            {
                var delta = (float)i / precision;
                var position = direction * delta;
                position.y += curve.Evaluate(delta);
                spline.Add(new(position));
            }

            //TODO: spline bug?
            //spline.SetTangentMode(TangentMode.AutoSmooth);
        }
        #endregion

        #region METHODS PUBLIC
        public void MagnetObjectsByCallbacks(Queue<Action<Transform>> callbacks)
        {
            var targets = FindTargets(callbacks.Count);

            foreach (var target in targets)
            {
                var callback = callbacks.Dequeue();
                SetupAttractorBeam(target, callback);
            }
        }
        #endregion

        #region COROUTINES
        IEnumerator AttractorBeam(Transform target, Action<Transform> callback)
        {
            var startPosition = target.position;
            
            var path = CreatePath();
            path.transform.position = startPosition;

            var pathT = 0f;
            var spline = path.Spline;
            while (true)
            {
                UpdateSpline(spline, startPosition, _magnetPoint.transform.position, _pathPrecision, _pathCurve);

                var pathSpeed = _magnetSpeed * Time.deltaTime;
                var targetPosition = (Vector3)spline.GetPointAtLinearDistance(pathT, pathSpeed, out pathT);
                targetPosition += path.transform.position;

                target.transform.position = targetPosition;

                if (pathT == 1f) break;
                yield return null;
            }

            Destroy(path.gameObject);
            callback?.Invoke(target);
        }
        #endregion
    }
}
