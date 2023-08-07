using EventHolder;
using UnityEngine;

namespace Gameplay
{
    public abstract class AScreenUiController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected GameObject _content;
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
