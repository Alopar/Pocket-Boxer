using UnityEngine;
using EventHolder;
using Manager;

namespace Gameplay
{
    [SelectionBase]
    public class TutorialMarker : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _view;

        [Space(10)]
        [SerializeField] private TutorialStep _tutorialStep;
        [SerializeField] private bool _inUI;
        #endregion

        #region HANDLERS
        private void h_TutorialStep(TutorialStepInfo info)
        {
            if(info.TutorialStep == _tutorialStep)
            {
                _view.SetActive(true);

                if (!_inUI)
                {
                    EventHolder<TutorialObservingInfo>.NotifyListeners(new(gameObject));
                }
            }
            else
            {
                _view.SetActive(false);
            }
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _view.SetActive(false);
        }

        private void OnEnable()
        {
            EventHolder<TutorialStepInfo>.AddListener(h_TutorialStep, true);
        }

        private void OnDisable()
        {
            EventHolder<TutorialStepInfo>.RemoveListener(h_TutorialStep);
        }

        private void LateUpdate()
        {
            if (_inUI) return;

            transform.rotation = Quaternion.identity;
        }
        #endregion
    }
}
