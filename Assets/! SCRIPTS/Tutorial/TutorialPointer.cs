using UnityEngine;
using EventHolder;

namespace Gameplay
{
    public class TutorialPointer : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _content;
        #endregion

        #region FIELDS PRIVATE
        private Transform _tutorialTarget;
        #endregion

        #region HANDLERS
        private void h_TutorialObserving(TutorialObservingInfo info)
        {
            if(info == null)
            {
                _content.SetActive(false);
                return;
            }

            _tutorialTarget = info.GameObject.transform;
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
            EventHolder<TutorialObservingInfo>.AddListener(h_TutorialObserving, true);
        }

        private void OnDisable()
        {
            EventHolder<TutorialObservingInfo>.RemoveListener(h_TutorialObserving);
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