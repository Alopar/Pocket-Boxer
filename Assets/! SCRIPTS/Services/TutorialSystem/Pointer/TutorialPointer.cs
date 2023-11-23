using UnityEngine;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{
    public class TutorialPointer : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _content;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ITutorialService _tutorialService;

        private Transform _tutorialTarget;
        #endregion

        #region HANDLERS
        private void TutorialMarkerChanged(TutorialSceneMarker marker)
        {
            if (marker == null)
            {
                _content.SetActive(false);
                return;
            }

            _tutorialTarget = marker.gameObject.transform;
            _content.SetActive(true);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            _tutorialService.OnMarkerChanged += TutorialMarkerChanged;
        }

        private void OnDisable()
        {
            _tutorialService.OnMarkerChanged -= TutorialMarkerChanged;
        }

        private void LateUpdate()
        {
            RotateAtTarget();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _content.SetActive(false);
        }

        private void RotateAtTarget()
        {
            if (_tutorialTarget == null) return;
            transform.LookAt(_tutorialTarget.transform, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        #endregion
    }
}
