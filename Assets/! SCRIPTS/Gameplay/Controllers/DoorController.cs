using UnityEngine;

namespace Gameplay
{
    public class DoorController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Animator _animator;
        #endregion

        #region UNITY CALLBACKS
        private void OnTriggerEnter(Collider other)
        {
            if (other.Tag() == Tag.Player)
            {
                _animator.Play("Open");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.Tag() == Tag.Player)
            {
                _animator.Play("Close");
            }
        }
        #endregion
    }
}
