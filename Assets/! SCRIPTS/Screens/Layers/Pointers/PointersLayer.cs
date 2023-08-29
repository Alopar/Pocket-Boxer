using System.Collections.Generic;
using UnityEngine;
using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class PointersLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Pointer _basePointerPrefab;
        #endregion

        #region FIELDS PRIVATE
        private Camera _camera;
        private PlayerController _player;

        private Dictionary<Transform, Pointer> _targetPointers = new();
        #endregion

        #region HANDLERS
        [EventHolder]
        private void PlayerSpawn(PlayerSpawnInfo info)
        {
            _player = info.PlayerController;
        }

        [EventHolder]
        private void TrackTarget(TrackTargetInfo info)
        {
            if (_targetPointers.ContainsKey(info.Target)) return;

            Pointer pointerPrefab = _basePointerPrefab;

            var pointer = Instantiate(pointerPrefab, _content.transform);
            _targetPointers.Add(info.Target, pointer);
        }

        [EventHolder]
        private void UntrackTarget(UntrackTargetInfo info)
        {
            if (!_targetPointers.ContainsKey(info.Target)) return;

            var pointer = _targetPointers[info.Target];
            _targetPointers.Remove(info.Target);

            if (pointer == null) return;

            Destroy(pointer.gameObject);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
        }

        private void Start()
        {
            FindCamera();
        }

        private void LateUpdate()
        {
            UpdatePointers();
        }
        #endregion

        #region METHODS PRIVATE
        private void FindCamera()
        {
            _camera = Camera.main;
        }

        private void UpdatePointers()
        {
            if (_player == null) return;

            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach (var pointer in _targetPointers)
            {
                MovePointer(pointer.Key.transform, _player.transform, pointer.Value, cameraPlanes);
            }
        }

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
