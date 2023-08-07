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
            }
            else
            {
                _content.SetActive(true);
                _tutorialTarget = info.GameObject.transform;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _content.SetActive(false);
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
            if (!_tutorialTarget) return;
            transform.LookAt(_tutorialTarget.transform, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        #endregion
    }
}