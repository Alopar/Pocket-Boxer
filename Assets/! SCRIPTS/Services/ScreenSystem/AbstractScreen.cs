using UnityEngine;
using Services.SignalSystem;
using Utility.DependencyInjection;

namespace Services.ScreenSystem
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected ScreenType _type;

        [Space(10)]
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected GameObject _content;
        #endregion

        #region FIELDS PRIVATE
        [Inject] protected ISignalService _signalService;
        #endregion

        #region PROPERTIES
        public ScreenType ScreenType => _type;
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

        #region METHODS PUBLIC
        public virtual void ShowScreen(object payload = null)
        {
            _content.SetActive(true);
        }

        public virtual void HideScreen()
        {
            _content.SetActive(false);
        }

        public virtual void CloseScreen()
        {
            HideScreen();
        }

        public void SetCanvasCamera(Camera camera)
        {
            _canvas.worldCamera = camera;
            _canvas.planeDistance = 1f;
        }
        #endregion
    }
}
