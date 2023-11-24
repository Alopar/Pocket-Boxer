using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PointerSystem
{
    public interface IPointerService
    {
        IEnumerable<Target> Targets { get; }

        event Action<Target> OnTargetAdd;
        event Action<Target> OnTargetRemove;

        void AddTarget(Transform transform, PointerType pointerType);
        void RemoveTarget(Transform transform);
    }
}
