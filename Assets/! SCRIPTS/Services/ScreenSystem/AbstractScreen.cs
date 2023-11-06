using UnityEngine;
using Services.SignalSystem;
using Utility.DependencyInjection;

namespace Services.ScreenSystem
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected GameObject _content;
        #endregion

        #region FIELDS PRIVATE
        [Inject] protected ISignalService _signalService;
        #endregion

        #region UNITY CALLBACKS
        protected virtual void OnEnable()
        {
            _signalService.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            _signalService.Unsubscribe(this);
        }
        #endregion

        #region METHODS PRIVATE
        protected virtual void ShowScreen()
        {
            _content.SetActive(true);
        }

        protected virtual void HideScreen()
        {
            _content.SetActive(false);
        }
        #endregion

        #region METHODS PUBLIC
        public virtual void CloseScreen()
        {
            HideScreen();
        }
        #endregion
    }
}
