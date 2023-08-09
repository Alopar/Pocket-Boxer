using UnityEngine;
using EventHolder;
using Services.TutorialSystem;

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
                if (_inUI) return;

                EventHolder<TutorialObservingInfo>.NotifyListeners(new(gameObject));
                return;
            }

            _view.SetActive(false);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            EventHolder<TutorialStepInfo>.AddListener(h_TutorialStep, true);
        }

        private void OnDisable()
        {
            EventHolder<TutorialStepInfo>.RemoveListener(h_TutorialStep);
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _view.SetActive(false);
        }
        #endregion
    }
}
