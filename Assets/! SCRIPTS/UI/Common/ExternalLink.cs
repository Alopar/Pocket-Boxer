using UnityEngine;

namespace Gameplay
{
    public class ExternalLink : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _URL;
        #endregion

        #region METHODS PUBLIC
        public void OpenURL()
        {
            Application.OpenURL(_URL);
        }
        #endregion
    }
}
