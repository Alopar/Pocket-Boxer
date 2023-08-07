using UnityEngine;

namespace Gameplay
{
    public interface IDamageable
    {
        public GameObject GameObject { get; }
        public bool DealDamage(uint damage);
    }
}