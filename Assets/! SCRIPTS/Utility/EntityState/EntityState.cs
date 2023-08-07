namespace EntityState
{
    public abstract class EntityState<Tentity> : IEntityState where Tentity : EntityMonoBehavior
    {
        #region FIELDS PRIVATE
        protected Tentity _entity;
        #endregion

        #region METHODS PUBLIC
        public virtual void Enter(EntityMonoBehavior entity)
        {
            _entity = entity as Tentity;
        }

        public virtual void Exit()
        {
            // void
        }
        #endregion
    }

    public interface IEntityState
    {
        public void Enter(EntityMonoBehavior entity);
        public void Exit();
    }
}