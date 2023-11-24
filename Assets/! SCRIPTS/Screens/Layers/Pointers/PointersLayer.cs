using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Services.ScreenSystem;
using Services.PointerSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class PointersLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private List<Pointer> _pointersPrefabs;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IPointerService _pointerService;

        private Camera _camera;
        private PlayerController _player;

        private Dictionary<Target, Pointer> _targetPointers = new();
        #endregion

        #region HANDLERS
        [Subscribe]
        private void PlayerSpawn(PlayerSpawn info)
        {
            _player = info.PlayerController;
        }

        private void TrackTarget(Target target)
        {
            CreatePointer(target);
        }

        private void UntrackTarget(Target target)
        {
            if (!_targetPointers.ContainsKey(target)) return;

            var pointer = _targetPointers[target];
            _targetPointers.Remove(target);

            if (pointer == null) return;

            Destroy(pointer.gameObject);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            FindCamera();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _pointerService.OnTargetAdd += TrackTarget;
            _pointerService.OnTargetRemove += UntrackTarget;

            ClearPointers();
            CreatePointers(_pointerService.Targets);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _pointerService.OnTargetAdd -= TrackTarget;
            _pointerService.OnTargetRemove -= UntrackTarget;

            ClearPointers();
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

        private void ClearPointers()
        {
            foreach (var pointer in _targetPointers)
            {
                if (pointer.Value == null) return;
                Destroy(pointer.Value.gameObject);
            }

            _targetPointers.Clear();
        }

        private void CreatePointer(Target target)
        {
            if (_targetPointers.ContainsKey(target)) return;

            var pointerPrefab = _pointersPrefabs.FirstOrDefault(e => e.Type == target.PointerType);
            if (pointerPrefab == null)
            {
                Debug.LogError($"Pointer by type: {target.PointerType} not found!");
                return;
            }

            var pointer = Instantiate(pointerPrefab, _content.transform);
            _targetPointers.Add(target, pointer);
        }

        private void CreatePointers(IEnumerable<Target> targets)
        {
            foreach (var target in targets)
            {
                CreatePointer(target);
            }
        }

        private void UpdatePointers()
        {
            if (_player == null) return;

            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach (var pointer in _targetPointers)
            {
                MovePointer(pointer.Key.Transform, _player.transform, pointer.Value, cameraPlanes);
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
