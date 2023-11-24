using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.PointerSystem
{
    public class PointerSystem : IPointerService
    {
        #region FIELDS PRIVATE
        private readonly List<Target> _targets = new();
        #endregion

        #region PROPERTIES
        public IEnumerable<Target> Targets => _targets;
        #endregion

        #region EVENTS
        public event Action<Target> OnTargetAdd;
        public event Action<Target> OnTargetRemove;
        #endregion

        #region METHODS PUBLIC
        public void AddTarget(Transform transform, PointerType pointerType)
        {
            var target = new Target(transform, pointerType);

            _targets.Add(target);
            OnTargetAdd?.Invoke(target);
        }

        public void RemoveTarget(Transform transform)
        {
            var target = _targets.FirstOrDefault(e => e.Transform == transform);
            if (target == null) return;

            _targets.Remove(target);
            OnTargetRemove?.Invoke(target);
        }
        #endregion
    }
}
