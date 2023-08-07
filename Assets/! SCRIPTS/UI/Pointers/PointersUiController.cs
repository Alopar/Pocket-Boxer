using System.Collections.Generic;
using UnityEngine;
using EventHolder;

namespace Gameplay
{
    public class PointersUiController : AScreenUiController
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Pointer _pointerPrefab;
        #endregion

        #region FIELDS PRIVATE
        private Camera _camera;
        private PlayerController _player;

        private Dictionary<Transform, Pointer> _targetPointers = new();
        #endregion

        #region HANDLERS
        private void h_PlayerSpawn(PlayerSpawnInfo info)
        {
            _player = info.PlayerController;
        }

        private void h_TrackTarget(TrackTargetInfo info)
        {
            var pointer = Instantiate(_pointerPrefab, _content.transform);
            _targetPointers.Add(info.Target, pointer);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            EventHolder<PlayerSpawnInfo>.AddListener(h_PlayerSpawn, true);
            EventHolder<TrackTargetInfo>.AddListener(h_TrackTarget, true);
        }

        private void OnDisable()
        {
            EventHolder<PlayerSpawnInfo>.RemoveListener(h_PlayerSpawn);
            EventHolder<TrackTargetInfo>.RemoveListener(h_TrackTarget);
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if(_player == null) return;

            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach(var pointer in _targetPointers)
            {
                MovePointer(pointer.Key.transform, _player.transform, pointer.Value, cameraPlanes);
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void MovePointer(Transform target, Transform player, Pointer pointer, Plane[] cameraPlanes)
        {
            var direction = target.position - player.position;
            var ray = new Ray(player.position, direction);

            var minDistance = Mathf.Infinity;
            for (int i = 0; i < 4; i++)
            {
                if (cameraPlanes[i].Raycast(ray, out float distance))
                {
                    minDistance = distance < minDistance ? distance : minDistance;
                }
            }

            pointer.gameObject.SetActive(direction.magnitude > minDistance);
            minDistance = Mathf.Clamp(minDistance, 0f, direction.magnitude);

            var worldPosition = ray.GetPoint(minDistance);
            var pointerPosition = _camera.WorldToScreenPoint(worldPosition);
            pointer.transform.position = Vector3.Lerp(pointer.transform.position, pointerPosition, 10f * Time.deltaTime);

            var playerPosition = _camera.WorldToScreenPoint(player.position);
            var pointerDirection = pointerPosition - playerPosition;

            var angle = Mathf.Atan2(pointerDirection.y, pointerDirection.x) * Mathf.Rad2Deg - 90f;
            pointer.Pivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            pointer.Icon.localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
        #endregion
    }
}