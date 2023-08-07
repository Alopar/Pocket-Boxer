using UnityEngine;

namespace EntityState
{
    public abstract class EntityMonoBehavior : MonoBehaviour
    {
        #region FIELDS PRIVATE
        protected IEntityState _state;
        #endregion

        #region METHODS PRIVATE
        protected void ChangeState(IEntityState state)
        {
            if (_state == null)
            {
                _state = state;
                _state.Enter(this);
                return;
            }

            if (_state.GetType().Name == state.GetType().Name) return;

            _state.Exit();
            _state = state;
            _state.Enter(this);
        }
        #endregion
    }
}